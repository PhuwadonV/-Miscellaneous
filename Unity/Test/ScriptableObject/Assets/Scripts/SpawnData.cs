using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn", menuName = "Object/Spawn", order = 1)]
public class SpawnData : ScriptableObject {
    public Color color;
    public Vector3[] spawnPoints;
}
