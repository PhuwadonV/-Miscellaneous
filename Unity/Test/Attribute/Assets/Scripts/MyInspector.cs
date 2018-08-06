using UnityEngine;

public class MyInspector : MonoBehaviour {
    [HideInInspector]
    public int Hidden;

    [SerializeField]
    private int MySerializePrivtae;

    [TextArea]
    public string MyTextArea;

    [Space(20)]
    public string A;

    [Header("Header")]
    public string B;

    [Multiline(5)]
    public string MyMultiline;

    [Range(0, 1)]
    public float MyRange;

    [Tooltip("Health value")]
    public float Health;
}
