using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSpawner : MonoBehaviour
{
    public static TrafficSpawner Current;

    public List<GameObject> cars;

    public GameObject trafficCar1, trafficCar2, trafficCar3, trafficCar4, trafficCar5, trafficCar6;


    void Start()
    {
        Current = this;
        cars = new List<GameObject>(Resources.LoadAll<GameObject>("Cars"));
        StartCoroutine(TrafficCarSpawner());
    }

    
    void Update()
    {
        
    }

    IEnumerator TrafficCarSpawner()
    {
        int random = Random.Range(0, cars.Count - 1);
        Instantiate(cars[random], new Vector3(gameObject.transform.position.x, cars[random].transform.position.y, gameObject.transform.position.z), gameObject.transform.rotation);

        while (true)
        {

            int randomCar = Random.Range(0, cars.Count - 1);
            //int randomCar = Random.Range(0, cars.Count - 1);
            float waitTime = Random.Range(2, 6);
            yield return new WaitForSecondsRealtime(waitTime);

            Instantiate(cars[randomCar], new Vector3(gameObject.transform.position.x, cars[randomCar].transform.position.y, gameObject.transform.position.z), gameObject.transform.rotation);




            
        }
    }

    void Instantiate(GameObject car)
    {
        Instantiate(car, new Vector3(gameObject.transform.position.x, car.transform.position.y, gameObject.transform.position.z), gameObject.transform.rotation);
    }
}
