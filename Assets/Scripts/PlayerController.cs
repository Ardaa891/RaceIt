using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;


public class PlayerController : MonoBehaviour
{
    public static PlayerController Current;
  
    void Start()
    {
        Current = this;
    }

    
    void Update()
    {
        if (LevelController.Current.gameActive)
        {
            if (Input.GetMouseButton(0) && !EventController.Current.crashed)
            {
                GetComponent<SplineFollower>().follow = true;
                GetComponent<SplineFollower>().followSpeed+=5;
                transform.GetChild(0).GetChild(1).GetComponent<Animator>().SetBool("Holding", true);
                transform.GetChild(0).GetChild(2).GetComponent<Animator>().SetBool("Holding", true);
                transform.GetChild(0).GetChild(3).GetComponent<Animator>().SetBool("Holding", true);
                transform.GetChild(0).GetChild(4).GetComponent<Animator>().SetBool("Holding", true);

                
                if (GetComponent<SplineFollower>().followSpeed >= PlayerPrefs.GetInt("MaxSpeed"))
                {
                    GetComponent<SplineFollower>().followSpeed = PlayerPrefs.GetInt("MaxSpeed");
                }
            }
            else
            {
                GetComponent<SplineFollower>().followSpeed-=5;
                transform.GetChild(0).GetChild(1).GetComponent<Animator>().SetBool("Holding", false);
                transform.GetChild(0).GetChild(2).GetComponent<Animator>().SetBool("Holding", false);
                transform.GetChild(0).GetChild(3).GetComponent<Animator>().SetBool("Holding", false);
                transform.GetChild(0).GetChild(4).GetComponent<Animator>().SetBool("Holding", false);

                if (GetComponent<SplineFollower>().followSpeed <= 0)
                {
                    GetComponent<SplineFollower>().followSpeed = 0;
                    GetComponent<SplineFollower>().follow = false;


                }
            }
        }



    }

    
}
