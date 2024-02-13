using Common;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class UnitMovementController : MonoBehaviour, IController
    {
        public bool IsMoving { get; private set; } = false;

        UnityAction onMovementComplete = null;
        UnityAction<BoardCoordinate> onMoved = null;
        private BoardCoordinate[] movePath = null;
        private int moveIndex = -1;
        private UnitController controller = null;
        private int moveSpeed = 1;
        private BoardCoordinate targetCoordinate = BoardCoordinate.Invalid;
        private IEnumerator currentMovement = null;
        private WaitForSeconds waitForSeconds = null;

        public void InitController() 
        {
            ResetVariables();
        }

        public void MoveForWandering(UnitController controller, int moveSpeed
            , BoardCoordinate startCoordinate, BoardCoordinate endCoordinate
            , UnityAction<BoardCoordinate> onMoved, UnityAction onMovementComplete)
        {
            StartMoving(controller, moveSpeed, startCoordinate, endCoordinate, onMoved, onMovementComplete);
        }

        public void MoveForAttacking(UnitController controller, int moveSpeed
            , BoardCoordinate startCoordinate, BoardCoordinate endCoordinate
            , UnityAction<BoardCoordinate> onMoved, UnityAction onMovementComplete) 
        {
            StartMoving(controller, moveSpeed, startCoordinate, endCoordinate, onMoved, onMovementComplete);
        }

        private void StartMoving(UnitController controller, int moveSpeed
            , BoardCoordinate startCoordinate, BoardCoordinate endCoordinate
            , UnityAction<BoardCoordinate> onMoved, UnityAction onMovementComplete) 
        {
            this.controller = controller;
            this.moveSpeed = moveSpeed;
            targetCoordinate = endCoordinate;
            this.onMoved = onMoved;
            this.onMovementComplete = onMovementComplete;

            movePath = Pathfinder.Instance.CalculatePathCoordinates(startCoordinate, targetCoordinate)?.ToArray();
            float moveInterval = 1f / moveSpeed;
            waitForSeconds = new WaitForSeconds(moveInterval);

            if (movePath == null || movePath.Length <1) 
            {
                CompleteMovement();
                return;
            }

            currentMovement = MoveSequence();

            GameBoardManager.Instance.OnObjectPlaced.AddListener(OnObjectPlaced);
            GameBoardManager.Instance.OnPlaceObjectUpdated.AddListener(OnPlaceObjectUpdated);

            StartMovement();
        }

        private void CompleteMovement() 
        {
            GameBoardManager.Instance.OnObjectPlaced.RemoveListener(OnObjectPlaced);
            GameBoardManager.Instance.OnPlaceObjectUpdated.RemoveListener(OnPlaceObjectUpdated);

            StopMovement();
            onMovementComplete?.Invoke();

            ResetVariables();
        }

        private void ResetVariables() 
        {
            onMovementComplete = null;
            onMoved = null;
            movePath = null;
            moveIndex = -1;
            moveSpeed = 1;
            targetCoordinate = BoardCoordinate.Invalid;
            currentMovement = null;
            waitForSeconds = null;
        }

        private IEnumerator MoveSequence() 
        {
            moveIndex = 0;
            while (moveIndex < movePath.Length)
            {
                BoardCoordinate coordinate = movePath[moveIndex];
                bool success = GameBoardManager.Instance.UpdatePlaceableCoordinate(controller.Coordinate, coordinate);

                if (!success)
                    break;

                onMoved?.Invoke(coordinate);
                moveIndex++;

                if (moveIndex > movePath.Length - 1)
                    break;
                else
                    yield return waitForSeconds;
            }

            CompleteMovement();
        }

        private void StartMovement() 
        {
            if (currentMovement == null)
                return;

            StartCoroutine(currentMovement);
        }

        private void StopMovement() 
        {
            if (currentMovement == null)
                return;

            StopCoroutine(currentMovement);
        }

        private void OnObjectPlaced(IPlaceable placedObject, IEnumerable<BoardCoordinate> coordinates)
        {
            if (controller.IsEqual(placedObject))
                return;

            bool isInWay = false;
            bool isTargetOccupied = false;
            foreach (BoardCoordinate coordinate in coordinates)
            {
                if (!IsCoordinateInMovePath(coordinate))
                    continue;

                isInWay = true;
                isTargetOccupied = coordinate == targetCoordinate;

                if (isTargetOccupied)
                    break;
            }

            if (!isInWay)
                return;

            if (isTargetOccupied)
            {
                StopMovement();
                CompleteMovement();
                return;
            }

            StopMovement();
            movePath = Pathfinder.Instance.CalculatePathCoordinates(controller.Coordinate, targetCoordinate).ToArray();
            StartMovement();
        }

        private void OnPlaceObjectUpdated(IPlaceable placedObject, BoardCoordinate coordinate)
        {
            if (controller.IsEqual(placedObject))
                return;

            if (moveIndex < 0)
                return;

            int nextMoveIndex = 0;
            if (moveIndex + moveSpeed > movePath.Length - 1)
                nextMoveIndex = movePath.Length - 1;
            else
                nextMoveIndex = moveIndex + moveSpeed;

            BoardCoordinate nextCoordinate = movePath[nextMoveIndex];

            if (nextCoordinate != coordinate)
                return;

            StopMovement();
            movePath = Pathfinder.Instance.CalculatePathCoordinates(controller.Coordinate, targetCoordinate).ToArray();
            StartMovement();

        }

        private bool IsCoordinateInMovePath(BoardCoordinate targetCoordinate)
        {
            if (movePath?.Length < 1)
                return false;

            foreach (BoardCoordinate coordinate in movePath)
            {
                if (coordinate == targetCoordinate)
                    return true;
            }

            return false;
        }
    }
}