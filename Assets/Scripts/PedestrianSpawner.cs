using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianSpawner : MonoBehaviour
{
    public static PedestrianSpawner Current;

    public GameObject pedestrian;
    
    
    void Start()
    {
        Current = this;
        StartCoroutine(SpawnPedestrian());
    }

    
    void Update()
    {
        
    }


    IEnumerator SpawnPedestrian()
    {
        while (true)
        {
            float waitTime = Random.Range(3, 8);
            int xPos = Random.Range(-4, 5);
            yield return new WaitForSecondsRealtime(waitTime);
        
            Instantiate(pedestrian, new Vector3(gameObject.transform.position.x + xPos,gameObject.transform.position.y, gameObject.transform.position.z), gameObject.transform.rotation);
            
            
        }
        
    }


    
}
