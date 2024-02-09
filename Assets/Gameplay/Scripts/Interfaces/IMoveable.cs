using UnityEngine;

namespace Gameplay
{
    public interface IMoveable
    {
        void Move(BoardCoordinate targetCoordinate);
    }
}