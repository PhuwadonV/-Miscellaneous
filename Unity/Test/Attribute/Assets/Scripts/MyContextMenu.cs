using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyContextMenu : MonoBehaviour {
    [ContextMenuItem("Reset", "DoSomething")]
    public int SomeVal;

    [ContextMenu("Do Something")]
    void DoSomething()
    {
        SomeVal = 0;
        Debug.Log("Perform operation");
    }
}
