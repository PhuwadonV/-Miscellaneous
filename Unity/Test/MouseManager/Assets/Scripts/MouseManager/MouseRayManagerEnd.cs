using UnityEngine;

public class MouseRayManagerEnd : MonoBehaviour
{
    [SerializeField]
    protected MouseRayManagerBegin _mouseRayManagerBegin;

    protected void FixedUpdate()
    {
        _mouseRayManagerBegin.IsUpdated = false;
    }

    protected void Reset()
    {
        _mouseRayManagerBegin = GameObject.FindObjectOfType<MouseRayManagerBegin>();
    }
}