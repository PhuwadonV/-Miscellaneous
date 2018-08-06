using UnityEngine;

public class MouseRayManagerBegin : MonoBehaviour
{
    public int LayerMask;
    public float RayDistance;
    public QueryTriggerInteraction QueryTriggerInteraction;

    [HideInInspector]
    public bool IsUpdated;

    protected void Reset()
    {
        LayerMask = 1;
        RayDistance = float.MaxValue;
        QueryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
    }

    private bool _isRaycastHit;
    private Ray _ray;
    private RaycastHit _raycastHit;

    protected void Start()
    {
        IsUpdated = false;
    }

    public bool GetMouseRaycast(out Ray ray, out RaycastHit raycastHit)
    {
        if(!IsUpdated)
        {
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            _isRaycastHit = Physics.Raycast(_ray, out _raycastHit, RayDistance, LayerMask, QueryTriggerInteraction);
            IsUpdated = true;
            
        }
        ray = _ray;
        raycastHit = _raycastHit;
        return _isRaycastHit;
    }
}