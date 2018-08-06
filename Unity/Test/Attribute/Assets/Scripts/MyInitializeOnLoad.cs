using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class MyInitializeOnLoad
{
    static MyInitializeOnLoad()
    {
        Debug.Log("InitializeOnLoad");
    }
}