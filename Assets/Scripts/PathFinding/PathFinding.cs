using System;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIANGLE_COST = 14;

    [SerializeField] private Transform _gridDebugObject;
    [SerializeField] private LayerMask _obstaclesLayerMask;

    public static PathFinding Instance { get; private set; }

    private int _width;
    private int _height;
    private int _cellSize;

    private GridSystem<PathNode> _gridSystem;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    public void Setup(int width, int height, int cellsize)
    {
        _width = width;
        _height = height;
        _cellSize = cellsize;

        _gridSystem = new GridSystem<PathNode>(_width, _height, _cellSize,
           (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
        _gridSystem.CreateGridDebugObjects(_gridDebugObject);

        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPosition = _gridSystem.GetWorldPosition(gridPosition);

                float offsetValue = 5;
                Vector3 originPosition = worldPosition + Vector3.down * offsetValue;
                float rayDistance = offsetValue * 2;
                if (!Physics.Raycast(originPosition, Vector3.up, rayDistance, _obstaclesLayerMask)) continue;

                GetNode(x, z).SetWalkable(false);
            }
        }
    }

    public List<GridPosition> FindPath(GridPosition startPosition, GridPosition endPosition)
    {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        PathNode startNode = _gridSystem.GetGridObject(startPosition);
        PathNode endNode = _gridSystem.GetGridObject(endPosition);
        openList.Add(startNode);

        for (int x = 0; x < _gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < _gridSystem.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = _gridSystem.GetGridObject(gridPosition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();
            }
        }

        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startPosition, endPosition));
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostPathNode(openList);

            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighBourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighBourNode)) continue;

                if (!neighBourNode.IsWalkable())
                {
                    closedList.Add(neighBourNode);
                    continue;
                }

                int tentativeGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighBourNode.GetGridPosition());

                if (tentativeGCost < neighBourNode.GetGCost())
                {
                    neighBourNode.SetCameFromPathNode(currentNode);
                    neighBourNode.SetGCost(tentativeGCost);
                    neighBourNode.SetHCost(CalculateDistance(neighBourNode.GetGridPosition(), endPosition));
                    neighBourNode.CalculateFCost();

                    if (!openList.Contains(neighBourNode))
                    {
                        openList.Add(neighBourNode);
                    }
                }
            }

        }

        return null;
    }

    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodes = new List<PathNode>();

        pathNodes.Add(endNode);

        PathNode currentPathNode = endNode;

        while (currentPathNode.GetCameFromPathNode() != null)
        {
            pathNodes.Add(currentPathNode.GetCameFromPathNode());
            currentPathNode = currentPathNode.GetCameFromPathNode();
        }

        pathNodes.Reverse();

        List<GridPosition> gridPositionPathList = new List<GridPosition>();

        foreach (PathNode pathNode in pathNodes)
        {
            gridPositionPathList.Add(pathNode.GetGridPosition());
        }

        return gridPositionPathList;
    }

    private int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
    {
        GridPosition gridPositionDistance = gridPositionA - gridPositionB;
        int xDistance = Mathf.Abs(gridPositionDistance.X);
        int zDistance = Mathf.Abs(gridPositionDistance.Z);
        int remain = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIANGLE_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remain;
    }

    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostPathNode = pathNodeList[0];

        foreach (PathNode pathNode in pathNodeList)
        {
            if (pathNode.GetFCost() < lowestFCostPathNode.GetFCost())
                lowestFCostPathNode = pathNode;
        }

        return lowestFCostPathNode;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        GridPosition currentGridPosition = currentNode.GetGridPosition();

        if (currentGridPosition.X - 1 >= 0)
        {
            //Left
            neighbourList.Add(GetNode(currentGridPosition.X - 1, currentGridPosition.Z + 0));

            if (currentGridPosition.Z + 1 < _gridSystem.GetHeight())
            {
                //LeftUp
                neighbourList.Add(GetNode(currentGridPosition.X - 1, currentGridPosition.Z + 1));
            }

            if (currentGridPosition.Z - 1 >= 0)
            {
                //LeftDown
                neighbourList.Add(GetNode(currentGridPosition.X - 1, currentGridPosition.Z - 1));
            }
        }

        if (currentGridPosition.X + 1 < _gridSystem.GetWidth())
        {
            //Right
            neighbourList.Add(GetNode(currentGridPosition.X + 1, currentGridPosition.Z + 0));

            if (currentGridPosition.Z + 1 < _gridSystem.GetHeight())
            {
                //RightUp
                neighbourList.Add(GetNode(currentGridPosition.X + 1, currentGridPosition.Z + 1));
            }

            if (currentGridPosition.Z - 1 >= 0)
            {
                //RightDown
                neighbourList.Add(GetNode(currentGridPosition.X + 1, currentGridPosition.Z - 1));
            }
        }

        if (currentGridPosition.Z + 1 < _gridSystem.GetHeight())
        {
            //Up
            neighbourList.Add(GetNode(currentGridPosition.X + 0, currentGridPosition.Z + 1));
        }

        if (currentGridPosition.Z - 1 >= 0)
        {
            //Down
            neighbourList.Add(GetNode(currentGridPosition.X + 0, currentGridPosition.Z - 1));
        }

        return neighbourList;
    }

    private PathNode GetNode(int x, int z)
    {
        return _gridSystem.GetGridObject(new GridPosition(x, z));
    }
}
