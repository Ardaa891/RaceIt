using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSpawner : MonoBehaviour
{
    public static TrainSpawner Current;

    public GameObject train;
    public bool trainSpawned;
    
    void Start()
    {
        Current = this;

        
    }

    
    void Update()
    {
        
    }


    public void InstantiateTrain()
    {
        trainSpawned = true;
        Instantiate(train, transform.position, transform.rotation);
    }
}
