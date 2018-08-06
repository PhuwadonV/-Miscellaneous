using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ExecuteCountDown : MonoBehaviour {
    public void Execute(UnityAction action, float second)
    {
        StartCoroutine(Countdonw(action, second));
    }

    IEnumerator Countdonw(UnityAction action, float second)
    {
        yield return new WaitForSeconds(second);
        action();
    }
}
