using UnityEngine;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour, ICanvasRaycastFilter, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private float _buttonRadius;

    [SerializeField]
    private float _moveableRadius;

    private Vector2 _direction;

    public Vector2 Direction
    {
        get
        {
            return _direction;
        }
        set
        {
            _direction = value;
        }
    }

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        float buttonRadius = _buttonRadius * UnityEngine.Screen.height / 600.0f;
        sp.x -= transform.position.x;
        sp.y -= transform.position.y;
        return sp.magnitude <= buttonRadius;
    }

    public void OnDrag(PointerEventData eventData)
    {
        float moveableRadius = _moveableRadius * UnityEngine.Screen.height / 600.0f;

        Vector2 originPos = transform.parent.position;
        Vector2 mousePos = Input.mousePosition;
        Vector2 pressPos = eventData.pressPosition;

        Vector2 pressToOrigin = originPos - pressPos;
        Vector2 originToNewPos = (mousePos + pressToOrigin) - originPos;

        float originToNewPosMag = originToNewPos.magnitude;
        if (originToNewPosMag > moveableRadius) originToNewPos *= moveableRadius / originToNewPosMag;

        transform.position = originPos + originToNewPos;
        _direction = originToNewPos / moveableRadius;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = transform.parent.position;
        _direction.x = 0;
        _direction.y = 0;
    }
}
