using UnityEngine;
using UnityEngine.UI;

public class ButtonListener : MonoBehaviour
{
    public UnityEngine.UI.Button button;

    void OnEnable()
    {
        button.onClick.AddListener(() => print("Listener : Click"));
    }

    void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }
}