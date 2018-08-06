using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Left : MonoBehaviour {
    public Spin spin;
    private Material material;

    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    void Update () {
		if(spin.transform.rotation.y > 0) material.color = Color.blue;
        else material.color = Color.white;
    }
}
