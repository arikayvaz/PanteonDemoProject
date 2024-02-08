using UnityEngine;

namespace Gameplay
{
    public class PathNode
    {
        public int x;
        public int y;

        public bool isWalkable;
        public int gCost;
        public int hCost;
        public int FCost => gCost + hCost;

        public PathNode prevNode;

        public PathNode(int x, int y, bool isWalkable)
        {
            this.x = x;
            this.y = y;
            this.isWalkable = isWalkable;
        }
    }
}