using UnityEngine;

public interface IMouseEvent
{
    bool OnMouseEventDown(ref Ray ray, ref RaycastHit raycastHit);
    bool OnMouseEventUp(ref Ray ray, ref RaycastHit raycastHit, Collider selectedCollider);
    bool OnMouseEventRelease(bool isReleaseOnCollider, ref Ray ray, ref RaycastHit raycastHit);
    void OnMouseEventSelected();
}