using UnityEngine;
using UnityEngine.Events;

public class Test : MonoBehaviour {
    public LoadScene loadScene; 
    void Start () {
        loadScene.OnLoaded += (UnityAction action) =>
        {
            //action();
        };
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
