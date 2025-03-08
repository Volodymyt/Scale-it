using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DraggableMap : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private RectTransform rectTransform;
    private bool isDragging = false;
    private Vector2 lastMousePosition;

    private static List<DraggableMap> allMaps = new List<DraggableMap>();

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        allMaps.Add(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        lastMousePosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        ResolveOverlap();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            Vector2 delta = eventData.position - lastMousePosition;
            rectTransform.anchoredPosition += delta;
            lastMousePosition = eventData.position;

            rectTransform.SetAsLastSibling();
        }
    }

    private void ResolveOverlap()
    {
        bool moved;
        int maxIterations = 10;

        for (int i = 0; i < maxIterations; i++)
        {
            moved = false;

            foreach (DraggableMap otherMap in allMaps)
            {
                if (otherMap == this) continue;

                if (IsOverlapping(otherMap))
                {
                    Vector2 offset = GetOffsetDirection(otherMap);
                    rectTransform.anchoredPosition = otherMap.rectTransform.anchoredPosition + offset;
                    moved = true;
                    break;
                }
            }

            if (!moved) break;
        }
    }

    private bool IsOverlapping(DraggableMap other)
    {
        Rect rect1 = GetWorldRect(rectTransform);
        Rect rect2 = GetWorldRect(other.rectTransform);
        return rect1.Overlaps(rect2);
    }

    private Rect GetWorldRect(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        return new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);
    }

    private Vector2 GetOffsetDirection(DraggableMap other)
    {
        Vector2 thisCenter = rectTransform.anchoredPosition;
        Vector2 otherCenter = other.rectTransform.anchoredPosition;
        Vector2 direction = (thisCenter - otherCenter).normalized;

        float offsetAmount = rectTransform.rect.width * 1.7f;
        return direction * offsetAmount;
    }

    private void OnDestroy()
    {
        allMaps.Remove(this);
    }
}
