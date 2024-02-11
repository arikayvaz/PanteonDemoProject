using System.Collections.Generic;
using UnityEngine;
using Gameplay.UnitControllerStateMachine;
using System.Collections;
using UnityEngine.UI;

namespace Gameplay
{
    public class UnitController : MonoBehaviour, IPickable, IPlaceable, ISelectable, IMoveable, IAttacker
    {
        public States CurrentState => stateMachine?.State?.StateId ?? States.None;

        [SerializeField] protected StateInfo stateInfo = new StateInfo();
        protected StateMachine stateMachine;

        public Vector2 Position
        {
            get
            {
                return GameBoardManager.Instance?.GetWorldPositionFromCoordinate(stateInfo?.viewModel?.Coordinate ?? BoardCoordinate.Invalid) ?? Vector2.zero;
            }
        }

        public BoardCoordinate Coordinate => stateInfo?.viewModel?.Coordinate ?? BoardCoordinate.Invalid;


        public void InitController(UnitModel model, BoardCoordinate coordinate)
        {
            stateInfo.viewModel = new UnitViewModel(model);

            UpdateCoordinate(coordinate);

            InitStateMachine();

            stateInfo.boardVisual.InitVisual(stateInfo.viewModel.SpriteUnit
                , stateInfo.viewModel.UnitColor
                , stateInfo.viewModel.CellSizeX
                , stateInfo.viewModel.CellSizeY
                , GameBoardManager.BoardSettings.CellSize);
        }

        public void UpdateVisualColor(Color colorUpdated)
        {
            stateInfo.boardVisual.UpdateColor(colorUpdated);
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
        }

        public IEnumerable<BoardCoordinate> GetPlaceCoordinates(bool includeSpawnPoint = false)
        {
            return GetPlaceCoordinates(stateInfo.viewModel.Coordinate);
        }

        private void InitStateMachine()
        {
            stateMachine = gameObject.AddComponent<StateMachine>();
            StateFactory stateFactory = new StateFactory();

            stateMachine.InitStateMachine(stateInfo, stateFactory.GetStates(stateMachine));
        }

        public void UpdateCoordinate(BoardCoordinate coordinate)
        {
            stateInfo.viewModel.UpdateCoordinate(coordinate);
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
            UnitController unit = selectable as UnitController;

            if (unit == null)
                return false;

            return unit == this;
        }

        public bool IsEqual(IPlaceable placeable) 
        {
            UnitController unit = placeable as UnitController;

            if (unit == null)
                return false;

            return unit == this;
        }

        public bool IsCoordinateInBounds(BoardCoordinate coordinate)
        {
            return false;
        }

        public void Move(BoardCoordinate targetCoordinate)
        {
            stateInfo.targetCoordinate = targetCoordinate;
            ChangeState(States.MovingToPosition);
        }

        public void Attack(IDamageable target) 
        {
            stateInfo.attackTarget = target;
            ChangeState(States.MovingToTarget);
        }
    }
}