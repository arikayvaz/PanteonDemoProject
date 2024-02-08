using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public interface IPlaceable
    {
        void Place(BoardCoordinate coordinate);

        IEnumerable<BoardCoordinate> GetPlaceCoordinates(BoardCoordinate origin);
        public IEnumerable<BoardCoordinate> GetPlaceCoordinates();
    }
}