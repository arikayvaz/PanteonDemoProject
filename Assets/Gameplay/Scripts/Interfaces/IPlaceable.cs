using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public interface IPlaceable
    {
        void Place(BoardCoordinate coordinate);

        IEnumerable<BoardCoordinate> GetPlaceCoordinates(BoardCoordinate origin, bool includeSpawnPoint = false);
        IEnumerable<BoardCoordinate> GetPlaceCoordinates(bool includeSpawnPoint = false);
        bool IsEqual(IPlaceable placeable);
        bool IsCoordinateInBounds(BoardCoordinate coordinate);
    }
}