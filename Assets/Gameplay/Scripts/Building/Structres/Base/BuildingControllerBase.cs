using Gameplay.BuildingControllerStateMachine;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace Gameplay
{
    public abstract class BuildingControllerBase : MonoBehaviour, IPickable, IPlaceable
    {
        public abstract BuildingTypes BuildingType { get; }

        public States CurrentState => stateMachine?.State?.StateId ?? States.None;

        [SerializeField] protected Transform trVisual = null;
        [SerializeField] protected StateInfo stateInfo = new StateInfo();
        protected StateMachine stateMachine;

        public Vector2 Position 
        {
            get 
            {
                return GameBoardManager.Instance?.GetWorldPositionFromCoordinate(stateInfo.coordinate) ?? Vector2.zero;
            }
        }

        public virtual void InitController(BuildingDataSO buildingData, BoardCoordinate coordinate) 
        {
            stateInfo.buildingData = buildingData;

            UpdateCoordinate(coordinate);

            InitStateMachine();

            SetVisualSize();
            UpdatePosition();
        }

        public void UpdateVisualColor(Color colorUpdated) 
        {
            stateInfo.spriteVisual.color = colorUpdated;
        }

        protected void ChangeState(States stateNew) 
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
            stateInfo.coordinate = coordinate;
        }

        private void SetVisualSize() 
        {
            trVisual.transform.localScale = new Vector3(stateInfo.buildingData.CellSizeX, stateInfo.buildingData.CellSizeY, 1f);
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
            gameObject.SetActive(false);
        }

        public void Place(BoardCoordinate placeCoordinate) 
        {
            if (CurrentState == States.Placed)
                return;

            UpdateCoordinate(placeCoordinate);
            ChangeState(States.Placed);
        }

        public IEnumerable<BoardCoordinate> GetPlaceCoordinates(BoardCoordinate origin)
        {
            if (stateInfo == null || stateInfo.buildingData == null)
                yield return BoardCoordinate.Invalid;

            for (int y = 0; y < stateInfo.buildingData.CellSizeY; y++)
            {
                for (int x = 0; x < stateInfo.buildingData.CellSizeX; x++)
                {
                    yield return new BoardCoordinate(origin.x + x, origin.y + y);
                }
            }
        }

        public IEnumerable<BoardCoordinate> GetPlaceCoordinates()
        {
            if (stateInfo == null || stateInfo.buildingData == null)
                yield return BoardCoordinate.Invalid;

            for (int y = 0; y < stateInfo.buildingData.CellSizeY; y++)
            {
                for (int x = 0; x < stateInfo.buildingData.CellSizeX; x++)
                {
                    yield return new BoardCoordinate(stateInfo.coordinate.x + x, stateInfo.coordinate.y + y);
                }
            }
        }
    }
}