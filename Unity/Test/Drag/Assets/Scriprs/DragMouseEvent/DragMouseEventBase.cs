using UnityEngine;

public abstract class DragMouseEventBase : MouseEventAdapter
{
    [SerializeField]
    private MouseRayManagerBegin _mouseRayManagerBegin;

    [SerializeField]
    private Vector3 _positionOffset;

    private Vector2 _lastmouseDownPos;

    protected abstract void OnDragBegin(ref Ray ray, ref RaycastHit raycastHit);
    protected abstract void OnHolding();
    protected abstract void OnDragging(bool isHitCollider , Vector3 collidePoint);
    protected abstract void OnDragEnd(bool isReleaseOnCollider, ref Ray ray, ref RaycastHit raycastHit);

    public override bool OnMouseEventDown(ref Ray ray, ref RaycastHit raycastHit)
    {
        _lastmouseDownPos = Input.mousePosition;
        gameObject.layer = 2;
        OnDragBegin(ref ray, ref raycastHit);
        return true;
    }

    public override bool OnMouseEventRelease(bool isReleaseOnCollider, ref Ray ray, ref RaycastHit raycastHit)
    {
        gameObject.layer = 0;
        OnDragEnd(isReleaseOnCollider, ref ray, ref raycastHit);
        return true;
    }

    public override void OnMouseEventSelected()
    {
        Vector2 pos = Input.mousePosition;
        if (pos == _lastmouseDownPos)
        {
            OnHolding();
            return;
        }

        Vector2 worldToScreenPoint = Camera.main.WorldToScreenPoint(transform.position + _positionOffset);
        Ray ray = Camera.main.ScreenPointToRay(pos + (worldToScreenPoint - _lastmouseDownPos));
        RaycastHit raycastHit;
        bool isHit = Physics.Raycast(ray, out raycastHit, _mouseRayManagerBegin.RayDistance, _mouseRayManagerBegin.LayerMask);
        _lastmouseDownPos = pos;
        OnDragging(isHit, isHit ? raycastHit.point - _positionOffset : Vector3.zero);
    }

    protected void Reset()
    {
        _mouseRayManagerBegin = GameObject.FindObjectOfType<MouseRayManagerBegin>();
    }
}
