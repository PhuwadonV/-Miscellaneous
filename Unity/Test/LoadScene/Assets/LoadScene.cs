using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {
    [SerializeField]
    private int _loadScene;

    [HideInInspector]
    public event UnityAction<UnityAction> OnLoaded;

    private float _progress;

    public float Progress
    {
        get
        {
            return _progress;
        }
        protected set
        {
            _progress = value;
        }
    }

    void Start () {
        StartCoroutine(LoadSceneAsynCoroutine(SceneManager.LoadSceneAsync(_loadScene)));
    }

    IEnumerator LoadSceneAsynCoroutine(AsyncOperation asynOp)
    {
        asynOp.allowSceneActivation = false;
        while (!asynOp.isDone)
        {
            _progress = asynOp.progress;
            if (_progress >= 0.9f) 
            {
                if (OnLoaded == null) asynOp.allowSceneActivation = true;
                else OnLoaded(() => { asynOp.allowSceneActivation = true; });
            }
            yield return null;
        }
    }
}
