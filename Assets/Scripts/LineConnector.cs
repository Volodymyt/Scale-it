using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LineConnector : MonoBehaviour, IPointerUpHandler
{
    private LineRenderer lineRenderer;
    private bool isDrawing = false;

    private void Start()
    {
        GameObject lineObj = new GameObject("DynamicLine");
        lineRenderer = lineObj.AddComponent<LineRenderer>();

        lineRenderer.sortingLayerName = "Card";
        lineRenderer.sortingOrder = 2;

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
        lineRenderer.positionCount = 2;
    }

    private void Update()
    {
        if (isDrawing)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 0f;
            lineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint( mousePosition));
        }
    }

    public void CanDrowLine()
    {
        isDrawing = true;

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDrawing = false;

        LineConnector targetButton = GetButtonUnderCursor(eventData);
        if (targetButton != null && targetButton != this)
        {
            lineRenderer.SetPosition(1, targetButton.gameObject.transform.position);
        }
        else
        {
            Destroy(lineRenderer.gameObject);
        }
    }

    private LineConnector GetButtonUnderCursor(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            LineConnector button = result.gameObject.GetComponent<LineConnector>();
            if (button != null)
            {
                return button;
            }
        }
        return null;
    }
}
