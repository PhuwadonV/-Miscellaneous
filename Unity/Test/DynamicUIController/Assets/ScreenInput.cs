using UnityEngine;
using UnityEngine.EventSystems;

public class ScreenInput : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private LineRenderer _lineRenderer;

    void Start()
    {
        _lineRenderer.SetPosition(0, Vector3.zero);
        _lineRenderer.SetPosition(1, Vector3.one);
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }
}
