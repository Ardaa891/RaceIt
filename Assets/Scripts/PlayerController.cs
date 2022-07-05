using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using MoreMountains.NiceVibrations;


public class PlayerController : MonoBehaviour
{
    public static PlayerController Current;
    public int touchCount;
  
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

                if (GetComponent<SplineFollower>().followSpeed <= PlayerPrefs.GetInt("MaxSpeed"))
                {
                    GetComponent<SplineFollower>().followSpeed += 300 * Time.deltaTime;
                    MMVibrationManager.Haptic(HapticTypes.MediumImpact);

                }


                transform.GetChild(0).GetChild(1).GetComponent<Animator>().SetBool("Holding", true);
                transform.GetChild(0).GetChild(2).GetComponent<Animator>().SetBool("Holding", true);
                transform.GetChild(0).GetChild(3).GetComponent<Animator>().SetBool("Holding", true);
                transform.GetChild(0).GetChild(4).GetComponent<Animator>().SetBool("Holding", true);



            }
            else
            {
                GetComponent<SplineFollower>().followSpeed-=500*Time.deltaTime;

                transform.GetChild(0).GetChild(1).GetComponent<Animator>().SetBool("Holding", false);
                transform.GetChild(0).GetChild(2).GetComponent<Animator>().SetBool("Holding", false);
                transform.GetChild(0).GetChild(3).GetComponent<Animator>().SetBool("Holding", false);
                transform.GetChild(0).GetChild(4).GetComponent<Animator>().SetBool("Holding", false);

                if (GetComponent<SplineFollower>().followSpeed <= 0)
                {
                    GetComponent<SplineFollower>().followSpeed = 0;
                    GetComponent<SplineFollower>().follow = false;


                }

                if(GetComponent<SplineFollower>().followSpeed > 0)
                {
                    MMVibrationManager.Haptic(HapticTypes.MediumImpact);
                }
           }
        }

       



    }

    
}
