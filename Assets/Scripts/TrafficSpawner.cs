using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSpawner : MonoBehaviour
{
    public static TrafficSpawner Current;

    public GameObject trafficCar1, trafficCar2, trafficCar3, trafficCar4, trafficCar5, trafficCar6;


    void Start()
    {
        Current = this;
        StartCoroutine(TrafficCarSpawner());
    }

    
    void Update()
    {
        
    }

    IEnumerator TrafficCarSpawner()
    {
        while (true)
        {
            //float waitTime = Random.Range(2, 6);
            yield return new WaitForSecondsRealtime(5f);

            int CarNum = Random.Range(1, 7);

            if(CarNum == 1)
            {
                //Instantiate(trafficCar1, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 3.75f, gameObject.transform.position.z), gameObject.transform.rotation);
                Instantiate(trafficCar1);
            }else if(CarNum == 2)
            {
                //Instantiate(trafficCar2, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 3.75f, gameObject.transform.position.z), gameObject.transform.rotation);
                Instantiate(trafficCar2);
            }
            else if(CarNum == 3)
            {
                //Instantiate(trafficCar3, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 3.75f, gameObject.transform.position.z), gameObject.transform.rotation);
                Instantiate(trafficCar3);
            }
            else if(CarNum == 4)
            {
                //Instantiate(trafficCar4, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 3.75f, gameObject.transform.position.z), gameObject.transform.rotation);
                Instantiate(trafficCar4);
            }else if(CarNum == 5)
            {
                Instantiate(trafficCar5);
            }else if(CarNum == 6)
            {
                Instantiate(trafficCar6);
            }



            
        }
    }

    void Instantiate(GameObject car)
    {
        Instantiate(car, new Vector3(gameObject.transform.position.x, car.transform.position.y, gameObject.transform.position.z), gameObject.transform.rotation);
    }
}
