using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private float _maxSearchRange = 10;
    [SerializeField] private float _gridSize = 110f;

    private int _padding = 1;

    private Dictionary<Vector2Int, int> _grid = new Dictionary<Vector2Int, int>();
    private Dictionary<int, List<Vector2Int>> _objectInGrid = new Dictionary<int, List<Vector2Int>>();

    public Vector2 PlaceOnGrid(int objectID, Vector2 position, Vector2 objectSizeDelta, Vector2 oldPosition)
    {
        Vector2Int gridPos = CordToGrid(position);
        Vector2 objectSize = objectSizeDelta / _gridSize;

        RemoveFromGrid(objectID);

        if (IsOccupied(gridPos, objectSize, _padding))
        {
            Vector2Int? newGridPos = FindNearestAvailablePosition(gridPos, objectSize);

            if (newGridPos.HasValue)
            {
                PlaceObjectOnGrid(objectID, newGridPos.Value, objectSize);
                return GridToCord(newGridPos.Value);
            }
            else
            {
                PlaceObjectOnGrid(objectID, CordToGrid(oldPosition), objectSize);
                return oldPosition;
            }
        }
        else
        {
            PlaceObjectOnGrid(objectID, gridPos, objectSize);
            return GridToCord(gridPos);
        }
    }

    private void RemoveFromGrid(int objectID)
    {
        if (!_objectInGrid.ContainsKey(objectID))
        {
            return;
        }

        foreach (var cell in _objectInGrid[objectID])
        {
            _grid.Remove(cell);
        }

        _objectInGrid.Remove(objectID);
    }

    private Vector2Int? FindNearestAvailablePosition(Vector2Int startGridPos, Vector2 objectSize)
    {
        Queue<Vector2Int> searchQueue = new Queue<Vector2Int>();
        HashSet<Vector2Int> visitedCells = new HashSet<Vector2Int>();

        searchQueue.Enqueue(startGridPos);
        visitedCells.Add(startGridPos);

        Vector2Int[] searchDirections = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
        int searchDepth = 0;

        while (searchQueue.Count > 0 && searchDepth < _maxSearchRange)
        {
            int currentLevelSize = searchQueue.Count;
            searchDepth++;

            for (int i = 0; i < currentLevelSize; i++)
            {
                Vector2Int currentPos = searchQueue.Dequeue();

                if (!IsOccupied(currentPos, objectSize, 0))
                {
                    return currentPos;
                }

                foreach (var direction in searchDirections)
                {
                    Vector2Int nextPos = currentPos + direction;

                    if (!visitedCells.Contains(nextPos))
                    {
                        searchQueue.Enqueue(nextPos);
                        visitedCells.Add(nextPos);
                    }
                }
            }
        }

        return null;
    }

    private bool IsOccupied(Vector2Int gridPos, Vector2 size, int padding)
    {
        for (int i = gridPos.x - padding; i <= gridPos.x + (int)size.x + padding; i++)
        {
            for (int j = gridPos.y - padding; j <= gridPos.y + (int)size.y + padding; j++)
            {
                if (_grid.ContainsKey(new Vector2Int(i, j)))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void MarkAreaOccupied(int objectID, Vector2Int gridPos, Vector2 objectSize)
    {
        if (!_objectInGrid.ContainsKey(objectID))
            _objectInGrid[objectID] = new List<Vector2Int>();

        for (int i = gridPos.x; i < gridPos.x + objectSize.x; i++)
        {
            for (int j = gridPos.y; j < gridPos.y + objectSize.y; j++)
            {
                Vector2Int cell = new Vector2Int(i, j);
                _grid[cell] = objectID;
                _objectInGrid[objectID].Add(cell);
            }
        }
    }

    private void PlaceObjectOnGrid(int objectID, Vector2Int gridPos, Vector2 objectSize)
    {
        MarkAreaOccupied(objectID, gridPos, objectSize);
    }

    private Vector2Int CordToGrid(Vector2 position)
    {
        return new Vector2Int(Mathf.RoundToInt(position.x / _gridSize), Mathf.RoundToInt(position.y / _gridSize));
    }

    private Vector2 GridToCord(Vector2Int gridPos)
    {
        return new Vector2(gridPos.x * _gridSize, gridPos.y * _gridSize);
    }
}