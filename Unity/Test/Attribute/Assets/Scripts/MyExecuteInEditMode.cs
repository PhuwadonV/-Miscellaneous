using UnityEngine;

[ExecuteInEditMode]
public class MyExecuteInEditMode : MonoBehaviour {
    [Delayed]
    public float speed;

	void Update () {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
	}
}