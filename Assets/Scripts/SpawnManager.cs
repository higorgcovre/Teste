using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform[] spawnsNormal;
    public Transform[] spawnsMasters;
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Instance.spawnsNormal = spawnsNormal;
        NetworkManager.Instance.spawnsMasters = spawnsMasters;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
