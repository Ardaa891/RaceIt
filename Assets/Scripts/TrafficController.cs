using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class TrafficController : MonoBehaviour
{
    public static TrafficController Current;

    public GameObject player;
    public double crashClipFrom;
    
    void Start()
    {
        Current = this;
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(DisAbleTrafficCar());
    }

   
    void Update()
    {
        transform.GetChild(1).GetComponent<Animator>().SetBool("Holding", true);
        transform.GetChild(2).GetComponent<Animator>().SetBool("Holding", true);
        transform.GetChild(3).GetComponent<Animator>().SetBool("Holding", true);
        transform.GetChild(4).GetComponent<Animator>().SetBool("Holding", true);

        


        transform.Translate(Vector3.forward * 60 * Time.deltaTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("Player"))
        {
            player.GetComponent<SplineFollower>().follow = false;
            player.GetComponent<SplineFollower>().followSpeed = 0f;
            player.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            player.transform.GetChild(0).GetChild(1).GetComponent<MeshRenderer>().enabled = false;
            player.transform.GetChild(0).GetChild(2).GetComponent<MeshRenderer>().enabled = false;
            player.transform.GetChild(0).GetChild(3).GetComponent<MeshRenderer>().enabled = false;
            player.transform.GetChild(0).GetChild(4).GetComponent<MeshRenderer>().enabled = false;
            player.transform.GetChild(1).transform.gameObject.SetActive(false);
            player.transform.GetChild(2).transform.gameObject.SetActive(false);
            
            

            StartCoroutine(SetActiveCar());

        }*/
    }


    IEnumerator SetActiveCar()
    {
        yield return new WaitForSecondsRealtime(3f);

        player.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        player.transform.GetChild(0).GetChild(1).GetComponent<MeshRenderer>().enabled = true;
        player.transform.GetChild(0).GetChild(2).GetComponent<MeshRenderer>().enabled = true;
        player.transform.GetChild(0).GetChild(3).GetComponent<MeshRenderer>().enabled = true;
        player.transform.GetChild(0).GetChild(4).GetComponent<MeshRenderer>().enabled = true;
        player.transform.GetChild(1).transform.gameObject.SetActive(true);
        player.transform.GetChild(2).transform.gameObject.SetActive(true);
        
        
        player.GetComponent<SplineFollower>().followSpeed = 0f;
       
        player.GetComponent<SplineFollower>().follow = true;
    }

    IEnumerator DisAbleTrafficCar()
    {
        yield return new WaitForSecondsRealtime(30f);

        gameObject.SetActive(false);
    }


    



}



