using UnityEngine;

public class MouseManagerAdapter : MouseManagerBase
{
    protected Collider _lastSelectedCollider;
    protected IMouseEvent _lastSelectedColliderMouseEvent;

    protected override bool OnMouseDownOnCollider(ref Ray ray, ref RaycastHit raycastHit)
    {
        IMouseEvent e = raycastHit.collider.GetComponent<IMouseEvent>();
        if (e != null && (e as MonoBehaviour).enabled) return e.OnMouseEventDown(ref ray, ref raycastHit);
        else return false;
    }
    protected override bool OnMouseUpOnCollider(ref Ray ray, ref RaycastHit raycastHit, Collider selectedCollider)
    {
        IMouseEvent e = raycastHit.collider.GetComponent<IMouseEvent>();
        if (e != null && (e as MonoBehaviour).enabled) return e.OnMouseEventUp(ref ray, ref raycastHit, selectedCollider);
        else return false;
    }
    protected override bool OnSelectedColliderRelease(bool isReleaseOnCollider, ref Ray ray, ref RaycastHit raycastHit, Collider selectedCollider)
    {
        IMouseEvent e = selectedCollider.GetComponent<IMouseEvent>();
        if (e != null && (e as MonoBehaviour).enabled) return e.OnMouseEventRelease(isReleaseOnCollider, ref ray, ref raycastHit);
        else return false;
    }
    protected override void OnSelectedCollider(Collider selectedCollider)
    {
        if (selectedCollider == _lastSelectedCollider) _lastSelectedColliderMouseEvent.OnMouseEventSelected();
        else
        {
            IMouseEvent e = selectedCollider.GetComponent<IMouseEvent>();
            e.OnMouseEventSelected();
            _lastSelectedCollider = selectedCollider;
            _lastSelectedColliderMouseEvent = e;  
        }
    }
    protected override void OnMouseDown(ref Ray ray) { }
    protected override void OnMouseUp(ref Ray ray, Collider selectedCollider) { }
}