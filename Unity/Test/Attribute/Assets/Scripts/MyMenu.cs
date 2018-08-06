using UnityEngine;
using UnityEditor;

public class MyMenu : MonoBehaviour {
    [MenuItem("MyMenu/Do Something")]
    static void DoSomething()
    {
        Debug.Log("Doing Something...");
    }

    [PreferenceItem("My Preferences")]

    public static void PreferencesGUI()
    {
     
    }
}