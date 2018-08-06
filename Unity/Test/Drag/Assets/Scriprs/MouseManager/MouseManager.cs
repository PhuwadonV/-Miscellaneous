using UnityEngine;

public class MouseManager : MouseManagerAdapter
{
    protected override void OnMouseDown(ref Ray ray)
    {
        Debug.Log("Down");
    }

    protected override void OnMouseUp(ref Ray ray, Collider selectedCollider)
    {
        Debug.Log("Up");
    }
}
