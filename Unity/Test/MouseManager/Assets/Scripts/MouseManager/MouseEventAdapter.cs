using UnityEngine;

public class MouseEventAdapter : MonoBehaviour, IMouseEvent
{
    public virtual bool OnMouseEventDown(ref Ray ray, ref RaycastHit raycastHit) { return false; }
    public virtual bool OnMouseEventUp(ref Ray ray, ref RaycastHit raycastHit, Collider selectedCollider) { return false; }
    public virtual bool OnMouseEventRelease(bool isReleaseOnCollider, ref Ray ray, ref RaycastHit raycastHit) { return false; }
    public virtual void OnMouseEventSelected() { }
}