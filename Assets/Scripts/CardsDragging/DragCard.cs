using UnityEngine.EventSystems;
using UnityEngine;

public class DragCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GridManager _gridManager;
    private RectTransform _rectTransform;

    private Vector2 _oldPosition = Vector2.zero;
    private Vector2 _originalPosition, _dragOffset;

    private bool _isDragging = false, _canDrag = false;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _gridManager = FindObjectOfType<GridManager>();
    }

    public void CardCanDrag()
    {
        _canDrag = true;
        _oldPosition = _rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_canDrag)
        {
            _originalPosition = _rectTransform.anchoredPosition;
            _dragOffset = eventData.position - new Vector2(_originalPosition.x, _originalPosition.y);
            _isDragging = true;
            _canDrag = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isDragging)
        {
            Vector2 currentPosition = eventData.position - _dragOffset;
            _rectTransform.anchoredPosition = currentPosition;
            _rectTransform.SetAsLastSibling();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;
        _rectTransform.anchoredPosition = _gridManager.PlaceOnGrid(gameObject.GetInstanceID(), _rectTransform.anchoredPosition, _rectTransform.sizeDelta, _oldPosition);
    }
}
