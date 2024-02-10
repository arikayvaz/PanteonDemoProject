using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public interface IPlaceable
    {
        void Place(BoardCoordinate coordinate);

        IEnumerable<BoardCoordinate> GetPlaceCoordinates(BoardCoordinate origin);
        IEnumerable<BoardCoordinate> GetPlaceCoordinates();
        bool IsEqual(IPlaceable placeable);
        bool IsCoordinateInBounds(BoardCoordinate coordinate);
    }
}