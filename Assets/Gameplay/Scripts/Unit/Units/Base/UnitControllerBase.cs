using System.Collections.Generic;
using UnityEngine;
using Gameplay.UnitControllerStateMachine;
using System.Collections;
using UnityEngine.UI;

namespace Gameplay
{
    public class UnitControllerBase : MonoBehaviour, IPickable, IPlaceable, ISelectable, IMoveable
    {
        [SerializeField] protected UnitTypes unitType = UnitTypes.None;

        public States CurrentState => stateMachine?.State?.StateId ?? States.None;

        [SerializeField] protected StateInfo stateInfo = new StateInfo();
        protected StateMachine stateMachine;

        public Vector2 Position
        {
            get
            {
                return GameBoardManager.Instance?.GetWorldPositionFromCoordinate(stateInfo.currentCoordinate) ?? Vector2.zero;
            }
        }


        public void InitController(UnitDataSO unitData, BoardCoordinate coordinate)
        {
            stateInfo.unitData = unitData;

            UpdateCoordinate(coordinate);

            InitStateMachine();

            SetVisualSize();
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
                    yield return new BoardCoordinate(stateInfo.currentCoordinate.x + x, stateInfo.currentCoordinate.y + y);
                }
            }
        }

        private void InitStateMachine()
        {
            stateMachine = gameObject.AddComponent<StateMachine>();
            StateFactory stateFactory = new StateFactory();

            stateMachine.InitStateMachine(stateInfo, stateFactory.GetStates(stateMachine));
        }

        public void UpdateCoordinate(BoardCoordinate coordinate)
        {
            stateInfo.currentCoordinate = coordinate;
            UpdatePosition();
        }

        public void HandleCoroutine(IEnumerator enumerator) 
        {
            StartCoroutine(enumerator);
        }

        public void TerminateCoroutine(IEnumerator enumerator) 
        {
            StopCoroutine(enumerator);
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

        public void Select()
        {
            ChangeState(States.Selected);
        }

        public void Deselect()
        {
            ChangeState(States.Idle);
        }

        public bool CanSelect() 
        {
            return CurrentState == States.Idle;
        }

        public bool IsEqual(ISelectable selectable) 
        {
            UnitControllerBase unit = selectable as UnitControllerBase;

            if (unit == null)
                return false;

            return unit == this;
        }

        public bool IsEqual(IPlaceable placeable) 
        {
            UnitControllerBase unit = placeable as UnitControllerBase;

            if (unit == null)
                return false;

            return unit == this;
        }

        public void Move(BoardCoordinate targetCoordinate)
        {
            stateInfo.targetCoordinate = targetCoordinate;
            stateInfo.movePath = Pathfinder.Instance.CalculatePathCoordinates(stateInfo.currentCoordinate, targetCoordinate).ToArray();

            ChangeState(States.MovingToPosition);
        }
    }
}