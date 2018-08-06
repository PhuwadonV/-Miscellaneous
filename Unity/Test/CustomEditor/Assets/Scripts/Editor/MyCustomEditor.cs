using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StatusBar))]
[CanEditMultipleObjects]
public class MyCustomEditor : Editor {
    float last_strength;
    float last_agility;
    float last_intelligent;

    SerializedProperty strength;
    SerializedProperty agility;
    SerializedProperty intelligent;

    void OnEnable()
    {
        strength = serializedObject.FindProperty("strength");
        agility = serializedObject.FindProperty("agility");
        intelligent = serializedObject.FindProperty("intelligent");
        last_strength = strength.floatValue;
        last_agility = agility.floatValue;
        last_intelligent = intelligent.floatValue;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.Slider(strength, 0.0f, 1.0f, new GUIContent("Strength"));
        EditorGUILayout.Slider(agility, 0.0f, 1.0f, new GUIContent("Agility"));
        EditorGUILayout.Slider(intelligent, 0.0f, 1.0f, new GUIContent("Intelligent"));

        if(strength.floatValue + agility.floatValue + intelligent.floatValue > 1.5f)
        {
            if (strength.floatValue != last_strength) strength.floatValue = last_strength;
            else if (agility.floatValue != last_agility) agility.floatValue = last_agility;
            else if (intelligent.floatValue != last_intelligent) intelligent.floatValue = last_intelligent;
        }

        last_strength = strength.floatValue;
        last_agility = agility.floatValue;
        last_intelligent = intelligent.floatValue;
        serializedObject.ApplyModifiedProperties();
    }
}
