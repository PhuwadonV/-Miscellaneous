using UnityEngine;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour, IPointerDownHandler, IPointerClickHandler,
    IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        print("Drag Begin : " + gameObject.name);
    }

    public void OnDrag(PointerEventData eventData)
    {
        print("Dragging : " + gameObject.name);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        print("Drag Ended : " + gameObject.name);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        print("Clicked : " + gameObject.name );
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        print("Mouse Down : " + gameObject.name);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("Mouse Enter : " + gameObject.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        print("Mouse Exit : " + gameObject.name);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        print("Mouse Up : " + gameObject.name);
    }
}