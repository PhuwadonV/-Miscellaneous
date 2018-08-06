using UnityEngine;

public class Spin : MonoBehaviour {
	void Update () {
        transform.Rotate(Vector3.up * 200.0f * Time.deltaTime);
	}
}
