using Common;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class Pathfinder : Singleton<Pathfinder>
    {
        //TODO: Optimize pathfinder 
        const int MOVE_STRAIGHT_COST = 10;
        const int MOVE_DIAGONAL_COST = 14;

        private List<PathNode> openList;
        private List<PathNode> closedList;

        private PathNode[,] grid;

        public void InitPathfinder(int width, int height) 
        {
            grid = new PathNode[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[x, y] = new PathNode(x, y, true);
                }
            }
        }

        public List<BoardCoordinate> CalculatePathCoordinates(BoardCoordinate start, BoardCoordinate end) 
        {
            List<PathNode> calculatedPath = FindPath(start, end);

            if (calculatedPath == null || calculatedPath.Count < 1)
                return null;

            List<BoardCoordinate> coordinates = new List<BoardCoordinate>();

            for (int i = 0; i < calculatedPath.Count; i++)
            {
                PathNode node = calculatedPath[i];

                if (node.x == start.x && node.y == start.y)
                    continue;

                coordinates.Add(new BoardCoordinate(node.x, node.y));
            }

            return coordinates;
        }

        public void UpdatePathNodeState(int x, int y, bool isWalkable) 
        {
            if (grid == null || grid.Length < 1)
                return;

            if (x > grid.GetLength(0) - 1 || y > grid.GetLength(1) - 1)
                return;

            PathNode node = grid[x, y];

            if (node == null)
                return;

            node.isWalkable = isWalkable;
        }

        private List<PathNode> FindPath(BoardCoordinate start, BoardCoordinate end) 
        {
            PathNode startNode = grid[start.x, start.y];
            PathNode endNode = grid[end.x, end.y];
            PathNode closestNode = null;

            openList = new List<PathNode>() { startNode };
            closedList = new List<PathNode>();

            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    PathNode node = grid[x, y];
                    node.gCost = int.MaxValue;
                    node.prevNode = null;
                }
            }//for (int x = 0; x < grid.GetLength(0); x++)

            startNode.gCost = 0;
            startNode.hCost = CalculateDistanceCost(startNode, endNode);

            while (openList.Count > 0)
            {
                PathNode currentNode = GetLowestFCostNode(openList);

                if (currentNode == endNode)
                    return CalculatePath(endNode);

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (PathNode neighbour in GetNeighbours(currentNode))
                {
                    if (closedList.Contains(neighbour))
                        continue;

                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbour);

                    if (tentativeGCost < neighbour.gCost) 
                    {
                        neighbour.prevNode = currentNode;
                        neighbour.gCost = tentativeGCost;
                        neighbour.hCost = CalculateDistanceCost(neighbour, endNode);

                        if (closestNode == null)
                            closestNode = neighbour;

                        if (neighbour.hCost < closestNode.hCost)
                            closestNode = neighbour;

                        if (!openList.Contains(neighbour))
                        {
                            openList.Add(neighbour);
                        }
                    }//if (tentativeGCost < neighbour.gCost)
                }//foreach (PathNode neighbour in GetNeighbours(currentNode))

            }//while (openList.Count > 0)

            if (closestNode != null)
                return CalculatePath(closestNode);

            return null;
        }

        private int CalculateDistanceCost(PathNode a, PathNode b) 
        {
            int distX = Mathf.Abs(a.x - b.x);
            int distY = Mathf.Abs(a.y - b.y);
            int remaining = Mathf.Abs(distX - distY);

            return MOVE_DIAGONAL_COST * Mathf.Min(distX, distY) + MOVE_STRAIGHT_COST * remaining;
        }

        private PathNode GetLowestFCostNode(List<PathNode> searchList) 
        {
            PathNode lowestNode = searchList[0];

            for (int i = 0; i < searchList.Count; i++)
            {
                if (searchList[i].FCost >= lowestNode.FCost)
                    continue;

                lowestNode = searchList[i];
            }

            return lowestNode;
        }

        private List<PathNode> CalculatePath(PathNode endNode) 
        {
            List<PathNode> path = new List<PathNode>();
            path.Add(endNode);

            PathNode currentNode = endNode;

            while (currentNode.prevNode != null)
            {
                path.Add(currentNode.prevNode);
                currentNode = currentNode.prevNode;
            }

            path.Reverse();
            return path;
        }

        private List<PathNode> GetNeighbours(PathNode centerNode) 
        {
            List<PathNode> neighbours= new List<PathNode>();
            PathNode searchNode = null;

            //Left
            if (centerNode.x - 1 >= 0)
            {
                searchNode = grid[centerNode.x - 1, centerNode.y];
                if (searchNode != null && searchNode.isWalkable)
                    neighbours.Add(searchNode);

                //Left Down
                if (centerNode.y - 1 >= 0) 
                {
                    searchNode = grid[centerNode.x - 1, centerNode.y - 1];
                    if (searchNode != null && searchNode.isWalkable)
                        neighbours.Add(searchNode);

                }

                //Left Up
                if (centerNode.y + 1 < grid.GetLength(1))
                {
                    searchNode = grid[centerNode.x - 1, centerNode.y + 1];
                    if (searchNode != null && searchNode.isWalkable)
                        neighbours.Add(searchNode);
                }

            }//if (centerNode.x -1 >= 0)

            //Right
            if (centerNode.x + 1 < grid.GetLength(0))
            {
                searchNode = grid[centerNode.x + 1, centerNode.y];
                if (searchNode != null && searchNode.isWalkable)
                    neighbours.Add(searchNode);

                //Right Down
                if (centerNode.y - 1 >= 0)
                {
                    searchNode = grid[centerNode.x + 1, centerNode.y - 1];
                    if (searchNode != null && searchNode.isWalkable)
                        neighbours.Add(searchNode);
                }

                //Right Up
                if (centerNode.y + 1 < grid.GetLength(1))
                {
                    searchNode = grid[centerNode.x + 1, centerNode.y + 1];
                    if (searchNode != null && searchNode.isWalkable)
                        neighbours.Add(searchNode);
                }
            }//if (centerNode.x + 1 < grid.GetLength(0))

            //Down
            if (centerNode.y - 1 >= 0)
            {
                searchNode = grid[centerNode.x, centerNode.y - 1];
                if (searchNode != null && searchNode.isWalkable)
                    neighbours.Add(searchNode);
            }

            //Up
            if (centerNode.y + 1 < grid.GetLength(1))
            {
                searchNode = grid[centerNode.x, centerNode.y + 1];
                if (searchNode != null && searchNode.isWalkable)
                    neighbours.Add(searchNode);
            }

            return neighbours;
        }

        /*
        private PathNode GetNearestNode(PathNode startNode, PathNode checkNode) 
        {
            PathNode nearestNode = null;
            List<PathNode> openList = new List<PathNode>();
            openList.Add(checkNode);
            List<PathNode> closedList = new List<PathNode>();

            while (openList.Count > 1)
            {
                PathNode currentNode = openList[0];

                if (currentNode == startNode)
                    break;

                openList.Remove(currentNode);

                foreach (PathNode neighbour in GetNeighbours(currentNode)) 
                {
                    if (closedList.Contains(neighbour))
                        continue;

                    if (neighbour.isWalkable)
                    {
                        nearestNode = neighbour;
                        break;
                    }

                    closedList.Add(neighbour);
                }
            }

            return nearestNode;
        }
        */
    }
}