using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    public static TrainController Current;

    public GameObject player;
   
    void Start()
    {
        Current = this;

        player = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(DisableTrain());
    }

    
    void Update()
    {
        transform.Translate(Vector3.right * 200 *Time.deltaTime);
    }

    IEnumerator DisableTrain()
    {
        yield return new WaitForSecondsRealtime(10f);

        gameObject.SetActive(false);
    }
}
