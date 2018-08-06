using UnityEngine;

public class SpawnObject : MonoBehaviour {
    public SpawnData spawnData;
    public GameObject gameObject;

    void Start () {
        foreach(Vector3 pos in spawnData.spawnPoints)
        {
            GameObject obj = Instantiate(gameObject);
            obj.GetComponent<MeshRenderer>().material.color = spawnData.color;
            obj.transform.position = pos;
            print(pos);
        }
    }
}
