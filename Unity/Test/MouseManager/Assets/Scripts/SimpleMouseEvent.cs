using UnityEngine;

public class SimpleMouseEvent : MouseEventAdapter
{
    [SerializeField]
    private UnityEngine.UI.Text _text;
    private uint _count;

    public override bool OnMouseEventDown(ref Ray ray, ref RaycastHit raycastHit)
    {
        Debug.Log("Down : " + raycastHit.collider.name);
        return true;
    }

    public override bool OnMouseEventUp(ref Ray ray, ref RaycastHit raycastHit, Collider selectedCollider)
    {
        Debug.Log("Up : " + raycastHit.collider.name);
        return true;
    }

    public override bool OnMouseEventRelease(bool isReleaseOnCollider, ref Ray ray, ref RaycastHit raycastHit)
    {
        _count = 0;
        _text.text = "";
        Debug.Log("Release : " + gameObject.name);
        return true;
    }

    public override void OnMouseEventSelected()
    {
        _count++;
        _text.text = gameObject.name + " : " + _count;
    }
}
