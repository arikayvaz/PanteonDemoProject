using System.Collections.Generic;
using UnityEngine;
using Gameplay.UnitControllerStateMachine;

namespace Gameplay
{
    public class UnitControllerBase : MonoBehaviour, IPickable, IPlaceable
    {
        [SerializeField] protected UnitTypes unitType = UnitTypes.None;

        public States CurrentState => stateMachine?.State?.StateId ?? States.None;

        [SerializeField] protected StateInfo stateInfo = new StateInfo();
        protected StateMachine stateMachine;

        public Vector2 Position
        {
            get
            {
                return GameBoardManager.Instance?.GetWorldPositionFromCoordinate(stateInfo.coordinate) ?? Vector2.zero;
            }
        }


        public void InitController(UnitDataSO unitData, BoardCoordinate coordinate)
        {
            stateInfo.unitData = unitData;

            UpdateCoordinate(coordinate);

            InitStateMachine();

            SetVisualSize();
            UpdatePosition();
        }

        public void UpdateVisualColor(Color colorUpdated)
        {
            stateInfo.spriteVisual.color = colorUpdated;
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
            ChangeState(States.Placed);
        }

        public IEnumerable<BoardCoordinate> GetPlaceCoordinates(BoardCoordinate origin)
        {
            if (stateInfo == null || stateInfo.unitData == null)
                yield return BoardCoordinate.Invalid;

            for (int y = 0; y < stateInfo.unitData.CellSizeY; y++)
            {
                for (int x = 0; x < stateInfo.unitData.CellSizeX; x++)
                {
                    yield return new BoardCoordinate(origin.x + x, origin.y + y);
                }
            }
        }

        public IEnumerable<BoardCoordinate> GetPlaceCoordinates()
        {
            if (stateInfo == null || stateInfo.unitData == null)
                yield return BoardCoordinate.Invalid;

            for (int y = 0; y < stateInfo.unitData.CellSizeY; y++)
            {
                for (int x = 0; x < stateInfo.unitData.CellSizeX; x++)
                {
                    yield return new BoardCoordinate(stateInfo.coordinate.x + x, stateInfo.coordinate.y + y);
                }
            }
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
            stateInfo.trVisual.localScale = new Vector3(stateInfo.unitData.CellSizeX, stateInfo.unitData.CellSizeY, 1f);
        }

        private void UpdatePosition()
        {
            transform.position = Position;
        }

        private void ChangeState(States stateNew)
        {
            stateMachine.ChangeState(stateNew);
        }
    }
}