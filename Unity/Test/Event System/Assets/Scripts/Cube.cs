using UnityEngine.EventSystems;
using UnityEngine;

public class Cube : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        print("Clicked : " + gameObject.name + " : " + eventData.pointerCurrentRaycast.worldPosition);
    }
}