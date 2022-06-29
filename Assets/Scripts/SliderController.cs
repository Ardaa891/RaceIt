using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public SplineComputer playerSpline;
    public LengthCalculator playerSplineLength;

    public Image sliderFillArea;

    public Slider playerSlider;
    public Slider aiSlider;

    public GameObject ai;

    public float playerMaxLength;
    public float aiMaxLength;
    public float targetLength;

    public float totalTravel;
    public float currentSpeed;
    public float aiCurrentSpeed;

    
    void Start()
    {
        ai = GameObject.FindGameObjectWithTag("AI");
        totalTravel = 0;
        playerMaxLength = GetComponent<LengthCalculator>().length;
        aiMaxLength = ai.GetComponent<LengthCalculator>().length;

        InvokeRepeating("SetSliderValues", 0.01f, 0.01f);
    }

    
    void Update()
    {
        currentSpeed = GetComponent<SplineFollower>().followSpeed;
        aiCurrentSpeed = ai.GetComponent<SplineFollower>().followSpeed;
        totalTravel += currentSpeed;
       
    }

    public void SetSliderValues()
    {
        playerSlider.value += ((currentSpeed / 100) / playerMaxLength);
        aiSlider.value += ((aiCurrentSpeed / 100) / aiMaxLength);

        
    }
}
