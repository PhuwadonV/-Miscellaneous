using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CoroutineService : MonoBehaviour
{
    private static WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();
    private static WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();

    IEnumerator DelayCallCoroutine(float secound, UnityAction callAfterDelay)
    {
        yield return new WaitForSeconds(secound);
        callAfterDelay();
    }

    IEnumerator DelayCallRealtimeCoroutine(float secound, UnityAction callAfterDelay)
    {
        yield return new WaitForSecondsRealtime(secound);
        callAfterDelay();
    }

    IEnumerator CountdownStateCoroutine(float countdown, float stop, Delegate<bool, float>.Type countdownState)
    {
        bool running = true;
        while (countdown > stop)
        {
            if (!countdownState(countdown))
            {
                running = false;
                break;
            }
            countdown -= Time.deltaTime;
            yield return _waitForEndOfFrame;
        }
        if(running) countdownState(stop);
    }

    IEnumerator UpdateCallCoroutine(Delegate<bool>.Type updateCall)
    {
        while (updateCall()) yield return _waitForEndOfFrame;
    }

    IEnumerator FixedUpdateCallCoroutine(Delegate<bool>.Type fixedUpdateCall)
    {
        while (fixedUpdateCall()) yield return _waitForFixedUpdate;
    }

    public void DelayCall(float secound, UnityAction callAfterDelay)
    {
        StartCoroutine(DelayCallCoroutine(secound, callAfterDelay));
    }

    public void DelayCallRealtime(float secound, UnityAction callAfterDelay)
    {
        StartCoroutine(DelayCallCoroutine(secound, callAfterDelay));
    }

    public void CountdownState(float countdown, float stop, Delegate<bool, float>.Type countdownState)
    {
        StartCoroutine(CountdownStateCoroutine(countdown, stop, countdownState));
    }

    public void UpdateCall(Delegate<bool>.Type updateCall)
    {
        StartCoroutine(UpdateCallCoroutine(updateCall));
    }

    public void FixedUpdateCall(Delegate<bool>.Type fixedUpdateCall)
    {
        StartCoroutine(FixedUpdateCallCoroutine(fixedUpdateCall));
    }
}