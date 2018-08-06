using System.Collections;
using UnityEngine.Events;
using UnityEngine;
using Class = TypeTrait.Class;

public class Test : MonoBehaviour {
    public ExecuteCountDown executeCountDown;
    public float countDown;

    void Start()
    {
        executeCountDown.Execute(PlaySpin(), countDown);
    }

    UnityAction PlaySpin()
    {
        Class.Tuple<bool> stop = new Class.Tuple<bool>(false);
        StartCoroutine(Spin(stop));
        return () =>
        {
            stop.data0 = true;
        };
    }

    IEnumerator Spin(Class.Tuple<bool> stop)
    {
        while (!stop.data0)
        {
            transform.Rotate(Vector3.up * 200.0f * Time.deltaTime);
            yield return null;
        }
    }
}
