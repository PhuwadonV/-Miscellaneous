using UnityEngine;

public class DragMouseEvent : DragMouseEventBase
{
    private Material _material;
    private Color _color = new Color();

    void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
    }

    protected override void OnDragBegin(ref Ray ray, ref RaycastHit raycastHit)
    {
        _color = _material.color;
        _color.a = 0.5f;
        _material.color = _color;
    }
    protected override void OnHolding()
    {

    }
    protected override void OnDragging(bool isHitCollider, Vector3 collidePoint)
    {
        if (isHitCollider) transform.position = collidePoint;
    }
    protected override void OnDragEnd(bool isReleaseOnCollider, ref Ray ray, ref RaycastHit raycastHit)
    {
        _color = _material.color;
        _color.a = 1.0f;
        _material.color = _color;
    }
}
