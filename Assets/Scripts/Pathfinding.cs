using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private List<PathNode> _openList;
    private List<PathNode> _closedList;

    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
    {
        Board.Instance.GetXY(startWorldPosition, out int startX, out int startY);
        Board.Instance.GetXY(endWorldPosition, out int endX, out int endY);

        List<PathNode> path = FindPath(startX, startY, endX, endY);
        if (path == null)
        {
            return null;
        }
        else
        {
            List<Vector3> vectorPath = new List<Vector3>();

            foreach (var pathNode in path)
            {
                vectorPath.Add(new Vector3(pathNode.X, 0, pathNode.Y));
            }
            return vectorPath;
        }
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        var startNode = Board.Instance.GetValue(startX, startY);
        var endNode = Board.Instance.GetValue(endX, endY);

        _openList = new List<PathNode> { startNode };
        _closedList = new List<PathNode>();

        for (int x = 0; x < Board.Instance.GetWidth; x++)
        {
            for (int y = 0; y < Board.Instance.GetWidth; y++)
            {
                var pathNode = Board.Instance.GetValue(x, y);
                pathNode.GCost = int.MaxValue;
                pathNode.CameFromNode = null;
            }
        }

        startNode.GCost = 0;
        startNode.HCost = TryGetDistanceCost(startNode, endNode);

        while (_openList.Count > 0)
        {
            var currentNode = GetLowestFCostNode(_openList);
            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            _openList.Remove(currentNode);
            _closedList.Add(currentNode);

            foreach (var neighbourNode in GetNeighbourList(currentNode))
            {
                if (_closedList.Contains(neighbourNode))
                    continue;
                if (!neighbourNode.IsWalkable)
                {
                    _closedList.Add(neighbourNode);
                    continue;
                }

                var tenativeGCost = currentNode.GCost + TryGetDistanceCost(currentNode, neighbourNode);
                if (tenativeGCost < neighbourNode.GCost)
                {
                    neighbourNode.CameFromNode = currentNode;
                    neighbourNode.GCost = tenativeGCost;
                    neighbourNode.HCost = TryGetDistanceCost(neighbourNode, endNode);

                    if (!_openList.Contains(neighbourNode))
                    {
                        _openList.Add(neighbourNode);
                    }
                }
            }
        }
        return null;

    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        if (currentNode.X - 1 >= 0)
        {
            // Left
            neighbourList.Add(GetNode(currentNode.X - 1, currentNode.Y));

            // Left Down
            if (currentNode.Y - 1 >= 0)
                neighbourList.Add(GetNode(currentNode.X - 1, currentNode.Y - 1));

            // Left Up
            if (currentNode.Y + 1 < Board.Instance.GetHeight)
                neighbourList.Add(GetNode(currentNode.X - 1, currentNode.Y + 1));
        }
        if (currentNode.X + 1 < Board.Instance.GetWidth)
        {
            // Right
            neighbourList.Add(GetNode(currentNode.X + 1, currentNode.Y));

            // Right Down
            if (currentNode.Y - 1 >= 0)
                neighbourList.Add(GetNode(currentNode.X + 1, currentNode.Y - 1));

            // Right Up
            if (currentNode.Y + 1 < Board.Instance.GetHeight)
                neighbourList.Add(GetNode(currentNode.X + 1, currentNode.Y + 1));
        }
        // Down
        if (currentNode.Y - 1 >= 0)
            neighbourList.Add(GetNode(currentNode.X, currentNode.Y - 1));

        // Up
        if (currentNode.Y + 1 < Board.Instance.GetHeight)
            neighbourList.Add(GetNode(currentNode.X, currentNode.Y + 1));

        return neighbourList;
    }

    public PathNode GetNode(int x, int y)
    {
        return Board.Instance.GetValue(x, y);
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        var path = new List<PathNode> { endNode };
        var currentNode = endNode;

        while (currentNode.CameFromNode != null)
        {
            path.Add(currentNode.CameFromNode);
            currentNode = currentNode.CameFromNode;
        }

        path.Reverse();
        return path;
    }

    private int TryGetDistanceCost(PathNode startNode, PathNode endNode)
    {
        if (endNode != null)
        {
            int xDistance = Mathf.Abs(startNode.X - endNode.X);
            int yDistance = Mathf.Abs(startNode.Y - endNode.Y);
            int remaining = Mathf.Abs(xDistance - yDistance);
            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }
        else
        {
            return -1;
        }
        
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        var lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].FCost < lowestFCostNode.FCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }
}

