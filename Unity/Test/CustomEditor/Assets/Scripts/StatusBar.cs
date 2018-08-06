using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusBar : MonoBehaviour {
    [Range(0, 1)]
    public float strength;

    [Range(0, 1)]
    public float agility;

    [Range(0, 1)]
    public float intelligent;

    void Reset()
    {
        strength = 0.5f;
        agility = 0.5f;
        intelligent = 0.5f;
    }
}
