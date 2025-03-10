using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DragCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform _rectTransform;
    private Vector2 _originalPosition;
    private Vector2 _dragOffset;
    private bool _isDragging = false;

    private const float _gridSize = 110f;
    private const int _padding = 1;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _originalPosition = _rectTransform.anchoredPosition;
        _dragOffset = eventData.position - new Vector2(_originalPosition.x, _originalPosition.y);
        _isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isDragging)
        {
            Vector2 currentPosition = eventData.position - _dragOffset;
            _rectTransform.anchoredPosition = currentPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;

        Vector2 targetPosition = GetSnappedPosition(_rectTransform.anchoredPosition);
        Vector2 finalPosition = FindNearestAvailablePosition(targetPosition);

        _rectTransform.anchoredPosition = finalPosition;
    }

    private Vector2 GetSnappedPosition(Vector2 position)
    {
        float x = Mathf.Round(position.x / _gridSize) * _gridSize;
        float y = Mathf.Round(position.y / _gridSize) * _gridSize;
        return new Vector2(x, y);
    }

    private Vector2 FindNearestAvailablePosition(Vector2 targetPosition)
    {
        if (!IsPositionOccupied(targetPosition, false))
            return targetPosition;

        Queue<Vector2> queue = new Queue<Vector2>();
        HashSet<Vector2> visited = new HashSet<Vector2>();

        queue.Enqueue(targetPosition);
        visited.Add(targetPosition);

        Vector2[] directions = {
            new Vector2(_gridSize, 0), new Vector2(-_gridSize, 0),
            new Vector2(0, _gridSize), new Vector2(0, -_gridSize),
            new Vector2(_gridSize, _gridSize), new Vector2(-_gridSize, -_gridSize),
            new Vector2(_gridSize, -_gridSize), new Vector2(-_gridSize, _gridSize)
        };

        while (queue.Count > 0)
        {
            Vector2 currentPos = queue.Dequeue();

            foreach (Vector2 dir in directions)
            {
                Vector2 newPos = currentPos + dir;

                if (!visited.Contains(newPos))
                {
                    visited.Add(newPos);
                    if (!IsPositionOccupied(newPos, true))
                    {
                        return newPos;
                    }
                    queue.Enqueue(newPos);
                }
            }
        }

        Debug.LogWarning("No valid position found!");
        return targetPosition;
    }

    private bool IsPositionOccupied(Vector2 position, bool checkPadding)
    {
        foreach (var otherCard in FindObjectsOfType<DragCard>())
        {
            if (otherCard != this)
            {
                RectTransform otherTransform = otherCard._rectTransform;
                Vector2 otherCardPosition = otherTransform.anchoredPosition;

                float width = otherTransform.rect.width;
                float height = otherTransform.rect.height;

                float offset = checkPadding ? (_gridSize * _padding) : 0f;

                for (int x = -2; x <= 2; x++)
                {
                    for (int y = -3; y <= 3; y++)
                    {
                        Vector2 cellPosition = position + new Vector2(x * _gridSize, y * _gridSize);

                        Rect occupiedAreaRight = new Rect(
                             otherCardPosition.x - width / 2 + _gridSize,
                             otherCardPosition.y - height / 2 + _gridSize,
                             width + _gridSize,
                             height + _gridSize
                         );

                        Rect occupiedAreaLeft = new Rect(
                            otherCardPosition.x - width / 2 - _gridSize - 50,
                            otherCardPosition.y - height / 2 - _gridSize - 50,
                            width + _gridSize / 2,
                            height + _gridSize / 2);

                        if (occupiedAreaRight.Contains(cellPosition))
                        {
                            if (occupiedAreaLeft.Contains(cellPosition))
                            {
                                Debug.Log("Cell is occupied: " + cellPosition);
                                return true;
                            }
                        }
                    }
                }
            }
        }

        return false;
    }
}
