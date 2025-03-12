using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private float _maxSearchRange = 10;
    [SerializeField] private float _gridSize = 110f;

    private int _padding = 1;

    private Dictionary<int, Dictionary<int, int>> _grid = new Dictionary<int, Dictionary<int, int>>();

    public void PlaceOnGrid(int objectID, Vector2 position, Vector2 objectSize, RectTransform cardRectTransform, Vector2 oldPosition)
    {
        var (x, y) = CordToGrid(position);

        RemoveFromGrid(objectID);

        if (IsOccupied(x, y, objectSize))
        {
            Vector2 newPosition = FindNearestAvailablePosition(x, y, objectSize, objectID, oldPosition);
            if (newPosition == oldPosition)
            {
                cardRectTransform.anchoredPosition = oldPosition;
                PlaceObjectOnGrid(objectID, oldPosition, objectSize);
            }
            else
            {
                cardRectTransform.anchoredPosition = newPosition;
            }
        }
        else
        {
            PlaceObjectOnGrid(objectID, position, objectSize);
            cardRectTransform.anchoredPosition = GridToCord(x, y);
        }
    }

    private void RemoveFromGrid(int objectID)
    {
        var cellsToRemove = new List<(int, int)>();

        foreach (var xEntry in _grid)
        {
            foreach (var yEntry in xEntry.Value)
            {
                if (yEntry.Value == objectID)
                {
                    cellsToRemove.Add((xEntry.Key, yEntry.Key));
                }
            }
        }

        foreach (var cell in cellsToRemove)
        {
            if (_grid.ContainsKey(cell.Item1))
            {
                _grid[cell.Item1].Remove(cell.Item2);

                if (_grid[cell.Item1].Count == 0)
                {
                    _grid.Remove(cell.Item1);
                }
            }
        }
    }

    private Vector2 FindNearestAvailablePosition(int startX, int startY, Vector2 objectSize, int objectID, Vector2 fallbackPosition)
    {
        Queue<(int, int)> searchQueue = new Queue<(int, int)>();
        HashSet<(int, int)> visitedCells = new HashSet<(int, int)>();

        searchQueue.Enqueue((startX, startY));
        visitedCells.Add((startX, startY));

        Vector2[] searchDirections = { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1) };
        int searchDepth = 0;

        while (searchQueue.Count > 0 && searchDepth < _maxSearchRange)
        {
            int currentLevelSize = searchQueue.Count;
            searchDepth++;

            for (int i = 0; i < currentLevelSize; i++)
            {
                var (currentX, currentY) = searchQueue.Dequeue();

                if (IsAreaFree(currentX, currentY, objectSize))
                {
                    MarkAreaOccupied(objectID, currentX, currentY, objectSize);
                    return GridToCord(currentX, currentY);
                }

                foreach (var direction in searchDirections)
                {
                    int nextX = currentX + (int)direction.x;
                    int nextY = currentY + (int)direction.y;

                    if (!visitedCells.Contains((nextX, nextY)))
                    {
                        searchQueue.Enqueue((nextX, nextY));
                        visitedCells.Add((nextX, nextY));
                    }
                }
            }
        }

        return fallbackPosition;
    }

    private bool IsOccupied(int x, int y, Vector2 size)
    {
        for (int i = x - (int)size.x - _padding; i <= x + (int)size.x + _padding; i++)
        {
            for (int j = y - (int)size.y - _padding; j <= y + (int)size.y + _padding; j++)
            {
                if (_grid.ContainsKey(i) && _grid[i].ContainsKey(j))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool IsAreaFree(int x, int y, Vector2 size)
    {
        for (int i = x; i < x + size.x; i++)
        {
            for (int j = y; j < y + size.y; j++)
            {
                if (_grid.ContainsKey(i) && _grid[i].ContainsKey(j))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void MarkAreaOccupied(int objectID, int x, int y, Vector2 objectSize)
    {
        for (int i = x; i < x + objectSize.x; i++)
        {
            for (int j = y; j < y + objectSize.y; j++)
            {
                if (!_grid.ContainsKey(i))
                {
                    _grid[i] = new Dictionary<int, int>();
                }

                _grid[i][j] = objectID;
            }
        }
    }

    private void PlaceObjectOnGrid(int objectID, Vector2 position, Vector2 objectSize)
    {
        var (x, y) = CordToGrid(position);
        MarkAreaOccupied(objectID, x, y, objectSize);
    }

    private (int, int) CordToGrid(Vector2 position)
    {
        return (Mathf.RoundToInt(position.x / _gridSize), Mathf.RoundToInt(position.y / _gridSize));
    }

    private Vector2 GridToCord(int x, int y)
    {
        return new Vector2(x * _gridSize, y * _gridSize);
    }
}