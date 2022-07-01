using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianSpawner : MonoBehaviour
{
    public static PedestrianSpawner Current;

    public List<GameObject> pedestrians;

    public GameObject pedestrian;
    
    
    void Start()
    {
        Current = this;
        StartCoroutine(SpawnPedestrian());
        pedestrians = new List<GameObject>(Resources.LoadAll<GameObject>("Prefabs"));
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
            int randomPedestrian = Random.Range(0, pedestrians.Count - 1);
            yield return new WaitForSecondsRealtime(waitTime);
        
            Instantiate(pedestrians[randomPedestrian], new Vector3(gameObject.transform.position.x + xPos,gameObject.transform.position.y, gameObject.transform.position.z), gameObject.transform.rotation);
            
            
        }
        
    }


    
}
