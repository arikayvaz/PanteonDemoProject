using Gameplay.BuildingControllerStateMachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public abstract class BuildingController : MonoBehaviour, IPickable, IPlaceable, ISelectable
    {
        public States CurrentState => stateMachine?.State?.StateId ?? States.None;

        [SerializeField] protected StateInfo stateInfo = new StateInfo();
        protected StateMachine stateMachine;

        public BuildingViewModel ViewModel => stateInfo?.viewModel;

        public Vector2 Position 
        {
            get 
            {
                return GameBoardManager.Instance?.GetWorldPositionFromCoordinate(stateInfo.viewModel.Coordinate) ?? Vector2.zero;
            }
        }

        public virtual void InitController(BuildingModel model, BoardCoordinate coordinate) 
        {
            stateInfo.viewModel = new BuildingViewModel(model);

            UpdateCoordinate(coordinate);

            InitStateMachine();

            SetVisualSize();
            UpdatePosition();
        }

        public void UpdateVisualColor(Color colorUpdated) 
        {
            stateInfo.spriteVisual.color = colorUpdated;
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

        private void SetVisualSize() 
        {
            stateInfo.trVisual.transform.localScale = new Vector3(stateInfo.viewModel.CellSizeX, stateInfo.viewModel.CellSizeY, 1f);
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

        public IEnumerable<BoardCoordinate> GetPlaceCoordinates(BoardCoordinate origin)
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
        }

        public IEnumerable<BoardCoordinate> GetPlaceCoordinates()
        {
            if (stateInfo == null || stateInfo.viewModel == null)
                yield return BoardCoordinate.Invalid;

            for (int y = 0; y < stateInfo.viewModel.CellSizeY; y++)
            {
                for (int x = 0; x < stateInfo.viewModel.CellSizeX; x++)
                {
                    yield return new BoardCoordinate(stateInfo.viewModel.Coordinate.x + x, stateInfo.viewModel.Coordinate.y + y);
                }
            }
        }

        public void Select()
        {
            ChangeState(States.Selected);
        }

        public void Deselect()
        {
            ChangeState(States.Placed);
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
    }
}