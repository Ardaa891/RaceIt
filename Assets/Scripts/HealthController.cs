using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public static HealthController Current;

    public Image health1, health2, health3;
    

    public int hitCount;
    
    void Start()
    {
        
        
    }

    
    void Update()
    {
        WheelIcons();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pedestrian"))
        {
            hitCount++;
            

        }

        if (other.CompareTag("Train"))
        {
            hitCount++;
        }

        
    }


    void WheelIcons()
    {
        if(hitCount == 1)
        {
            var alphaColor1 = health1.color;
            alphaColor1.a = 0.25f;
            health1.color = alphaColor1;
        }else if(hitCount == 2)
        {
            var alphaColor1 = health1.color;
            alphaColor1.a = 0.25f;
            health1.color = alphaColor1;

            var alphaColor2 = health2.color;
            alphaColor2.a = 0.25f;
            health2.color = alphaColor2;
        }else if(hitCount == 3)
        {
            var alphaColor1 = health1.color;
            alphaColor1.a = 0.25f;
            health1.color = alphaColor1;

            var alphaColor2 = health2.color;
            alphaColor2.a = 0.25f;
            health2.color = alphaColor2;

            var alphaColor3 = health3.color;
            alphaColor3.a = 0.25f;
            health3.color = alphaColor3;

            LevelController.Current.GameOver();
        }
    }
}
