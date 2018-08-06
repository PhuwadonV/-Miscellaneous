using UnityEngine;

public class UpdateLog : MonoBehaviour {
    void Start()
    {
        print("Start" + gameObject.name);
    }

    void Update () {
        print("Update" + gameObject.name);
    }
}
