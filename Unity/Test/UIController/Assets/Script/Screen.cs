using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screen : MonoBehaviour {
	void Start () {
        print(UnityEngine.Screen.currentResolution);
        print(UnityEngine.Screen.width / 800.0f);
        print(UnityEngine.Screen.height / 600.0f);
    }
}
