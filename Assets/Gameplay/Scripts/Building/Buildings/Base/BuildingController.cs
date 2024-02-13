using Gameplay.BuildingControllerStateMachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Profiling.HierarchyFrameDataView;

namespace Gameplay
{
    public abstract class BuildingController : MonoBehaviour, IPickable, IPlaceable, ISelectable, IDamageable
    {
        public States CurrentState => stateMachine?.State?.StateId ?? States.None;

        [SerializeField] protected StateInfo stateInfo = new StateInfo();
        protected StateMachine stateMachine;

        public BuildingViewModel ViewModel => stateInfo?.viewModel;
        public BoardCoordinate SpawnPointCoordinate 
        {
            get 
            {
                return ViewModel.IsProduceUnits ? (stateInfo?.spawnPoint?.SpawnCoordinate ?? BoardCoordinate.Invalid) : BoardCoordinate.Invalid;
            }
        }

        public Vector2 Position 
        {
            get 
            {
                return GameBoardManager.Instance?.GetWorldPositionFromCoordinate(stateInfo.viewModel.Coordinate) ?? Vector2.zero;
            }
        }

        public Vector2 SpawnPointPosition 
        {
            get 
            {
                return GameBoardManager.Instance?.GetWorldPositionFromCoordinate(stateInfo?.spawnPoint?.SpawnCoordinate ?? BoardCoordinate.Invalid) ?? Vector2.zero;
            }
        }

        public virtual void InitController(BuildingModel model, BoardCoordinate coordinate) 
        {
            stateInfo.viewModel = new BuildingViewModel(model);

            UpdateCoordinate(coordinate);

            InitStateMachine();

            InitBoardVisual();
            InitBuildingNameText();

            UpdatePosition();

            if (stateInfo.viewModel.IsProduceUnits)
            {
                stateInfo.spawnPoint.InitSpawnPoint(stateInfo.viewModel.SpawnPointCoordinate
                , 1
                , 1
                , GameBoardManager.BoardSettings.CellSize);
            }
        }

        public void UpdateVisualColor(Color colorUpdated) 
        {
            stateInfo.boardVisual.UpdateColor(colorUpdated);
        }

        private void ChangeState(States stateNew) 
        {
            stateMachine.ChangeState(stateNew);
        }

        private void InitStateMachine() 
        {
            stateMachine = gameObject.AddComponent<StateMachine>();
            StateFactory stateFactory = new StateFactory();

            stateMachine.InitStateMachine(stateInfo, stateFactory.GetStates(stateMachine));
        }

        private void UpdateCoordinate(BoardCoordinate coordinate)
        {
            stateInfo.viewModel.UpdateCoordinate(coordinate);
        }

        private void InitBoardVisual() 
        {
            stateInfo.boardVisual.InitVisual(stateInfo.viewModel.SpriteBuilding
                , stateInfo.viewModel.BuildingColor
                , stateInfo.viewModel.CellSizeX
                , stateInfo.viewModel.CellSizeY
                , GameBoardManager.BoardSettings.CellSize);
        }

        private void InitBuildingNameText() 
        {
            stateInfo.textBuildingName.text = stateInfo.viewModel.Name;

            RectTransform rt = stateInfo.textBuildingName.transform as RectTransform;

            float width = stateInfo.viewModel.CellSizeX * GameBoardManager.BoardSettings.CellSize;
            float height = stateInfo.viewModel.CellSizeY * GameBoardManager.BoardSettings.CellSize;

            rt.sizeDelta = new Vector2(width, height);
            rt.localPosition = new Vector3(width * 0.5f, height * 0.5f, 0f);
        }

        private void UpdatePosition() 
        {
            transform.position = Position;
        }

        public void Pick()
        {
            if (CurrentState == States.Picked)
                return;

            ChangeState(States.Picked);
        }

        public void Drop()
        {
            if (CurrentState != States.Picked)
                return;

            ChangeState(States.None);
            gameObject.SetActive(false); //TODO: Back to pooler logic
        }

        public void Place(BoardCoordinate coordinate) 
        {
            if (CurrentState == States.Placed)
                return;

            UpdateCoordinate(coordinate);
            UpdatePosition();
            UpdateVisualColor(stateInfo.viewModel.BuildingColor);

            ChangeState(States.Placed);
        }

        public IEnumerable<BoardCoordinate> GetPlaceCoordinates(BoardCoordinate origin, bool includeSpawnPoint = false)
        {
            if (stateInfo == null || stateInfo.viewModel == null)
                yield return BoardCoordinate.Invalid;

            for (int y = 0; y < stateInfo.viewModel.CellSizeY; y++)
            {
                for (int x = 0; x < stateInfo.viewModel.CellSizeX; x++)
                {
                    yield return new BoardCoordinate(origin.x + x, origin.y + y);
                }
            }


            if (includeSpawnPoint && stateInfo.viewModel.IsProduceUnits)
                yield return new BoardCoordinate(origin.x + stateInfo.viewModel.SpawnPointOffsetCoordinate.x, origin.y + stateInfo.viewModel.SpawnPointOffsetCoordinate.y);
            
        }

        public IEnumerable<BoardCoordinate> GetPlaceCoordinates(bool includeSpawnPoint = false)
        {
            return GetPlaceCoordinates(stateInfo.viewModel.Coordinate, includeSpawnPoint);
        }

        public void Select()
        {
            ChangeState(States.Selected);

            GameUIController.Instance.ShowBuildingInformationPanel(ViewModel.Name, ViewModel.SpriteBuilding, ViewModel.BuildingColor);

            if (ViewModel.IsProduceUnits)
                UnitManager.Instance.ShowProducibleUnitInformation(ViewModel.GetProducibleUnits());
        }

        public void Deselect()
        {
            ChangeState(States.Placed);

            GameUIController.Instance.HideBuildingInformationPanel();
            
            if (ViewModel.IsProduceUnits)
                GameUIController.Instance.HideProducibleUnitInformationPanel();
        }

        public bool CanSelect() 
        {
            return CurrentState == States.Placed;
        }

        public bool IsEqual(ISelectable selectable) 
        {
            BuildingController building = selectable as BuildingController;

            if (building == null)
                return false;

            return building == this;
        }

        public bool IsEqual(IPlaceable placeable) 
        {
            BuildingController building = placeable as BuildingController;

            if (building == null)
                return false;

            return building == this;
        }

        public bool IsCoordinateInBounds(BoardCoordinate coordinate) 
        {
            bool isInXBounds = coordinate.x >= stateInfo.viewModel.Coordinate.x && coordinate.x <= stateInfo.viewModel.Coordinate.x;
            bool isInYBounds = coordinate.y >= stateInfo.viewModel.Coordinate.y && coordinate.y <= stateInfo.viewModel.Coordinate.y;

            return isInXBounds && isInYBounds;
        }

        public int GetHealth()
        {
            return stateInfo.viewModel.Health;
        }

        public void SetHealth(int health)
        {
            stateInfo.viewModel.SetHealth(health);
        }

        public void AddHealth(int healthDelta) 
        {
            stateInfo.viewModel.AddHealth(healthDelta);
        }

        public void OnDied()
        {
            BuildingManager.Instance.RemoveBuilding(this);

            //TODO: Add get back pooler logic
            ChangeState(States.None);
            gameObject.SetActive(false);
        }

        public BoardCoordinate GetCoordinate() 
        {
            return stateInfo.viewModel.Coordinate;
        }

        public BoardCoordinate GetAttackableCoordinate() 
        {
            return GameBoardManager.Instance.GetClosestCoordinateFromArea(GetPlaceCoordinates(), true);
        }
    }
}