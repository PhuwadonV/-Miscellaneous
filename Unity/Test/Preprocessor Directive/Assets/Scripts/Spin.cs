using UnityEngine;

public class Spin : MonoBehaviour {
	void Start () {
        Material mat = GetComponent<MeshRenderer>().material;
#if UNITY_EDITOR
        mat.color = Color.red;
#elif UNITY_STANDALONE_WIN
        mat.color = Color.blue;
#elif UNITY_WEBGL
        mat.color = Color.green;
#endif
    }
}
