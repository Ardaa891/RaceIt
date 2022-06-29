using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using MoreMountains.NiceVibrations;
using DG.Tweening;
using UnityEngine.UI;
public class EventController : MonoBehaviour
{
    public static EventController Current;

    public GameObject pedestrianSpawner1, pedestrianSpawner2;
    public GameObject player;
    public float currentDistance;
    public float yRot;
    public float delayValue;
    public bool drift;
    public bool crashed;
    public bool playerFinished;
    public bool acceleration;
    public bool stop;
    public Vector3 spawnPoint;
    public SplineComputer playerSpline1;
    public float splineLenght;
    public double travel;
    float xRot;
    public GameObject railRoadGate1, railRoadGate2;
    public Image sliderFillArea;
    
    void Start()
    {
        Current = this;
        player = GameObject.FindGameObjectWithTag("Player");
        drift = false;
        delayValue = 0.1f;
        acceleration = true;
        stop = false;
        splineLenght = (float)playerSpline1.CalculateLength();
        
    }

   
    void Update()
    {
        xRot = GetComponent<SplineFollower>().motion.rotationOffset.x;
        

        if (Input.GetMouseButton(0) && LevelController.Current.gameActive)
        {
            stop = true;
            
              if(xRot > -2 && acceleration)
              {
                xRot-=0.15f;
                GetComponent<SplineFollower>().motion.rotationOffset = new Vector3(xRot, transform.rotation.y);

              }  

              else if(xRot <= -2 && acceleration)
              {
                
                StartCoroutine(RiseUpCar());
                acceleration = false;
                
                
              }

              else if (!acceleration && xRot<=0)
              {
                xRot += 0.15f;
                GetComponent<SplineFollower>().motion.rotationOffset = new Vector3(xRot, transform.rotation.y);
              }
           


        }
        else
        {
            acceleration = true;
            Debug.Log("else");


            if (xRot < 2 && stop)
            {
                xRot += 0.15f;
                GetComponent<SplineFollower>().motion.rotationOffset = new Vector3(xRot, transform.rotation.y);
            }
            else if(GetComponent<SplineFollower>().motion.rotationOffset.x >= 0f)
            {
                Debug.Log("stop");
                stop = false;
                xRot -= 0.15f;
                GetComponent<SplineFollower>().motion.rotationOffset = new Vector3(xRot, transform.rotation.y);
            }


            


        }

    
            
           

        




    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StartPedestrian"))
        {
            pedestrianSpawner1.SetActive(true);
            pedestrianSpawner2.SetActive(true);
        }

        if (other.CompareTag("TrainTrigger"))
        {
            if (!TrainSpawner.Current.trainSpawned)
            {
                TrainSpawner.Current.InstantiateTrain();
                railRoadGate1.transform.DOLocalRotate(new Vector3(0, -90, 90), 1f, RotateMode.Fast).SetEase(Ease.Linear).SetDelay(1.5f);
                railRoadGate2.transform.DOLocalRotate(new Vector3(0, -90, 90), 1f, RotateMode.Fast).SetEase(Ease.Linear).SetDelay(1.5f);
                StartCoroutine(RailRoadGates());
            }
            
        }

        if (other.CompareTag("Train"))
        {
            crashed = true;

            DisableCar();

            InvokeRepeating("TurnOffMeshes", 0.2f, 0.2f);
            InvokeRepeating("TurnOnMeshes", 0.3f, 0.3f);

            StartCoroutine(TransformCar());

            //StartCoroutine(Crash());

        }

        if (other.CompareTag("FinishTrigger"))
        {
            playerFinished = true;
            GetComponent<SplineFollower>().enabled = false;
            Camera.main.GetComponent<CameraFollower>().enabled = false;
            FinishAnim();
            
            
        }

        
    }

    private void OnTriggerStay(Collider other)
    {
        /*if (other.CompareTag("drift"))
        {
            drift = true;
            Debug.Log("DRIFT");
            GetComponent<SplineFollower>().motion.rotationOffset = new Vector3(0, yRot, 0);
            if (other.gameObject.layer == 6)
            {
                yRot++;

                if (yRot >= 30)
                {
                    yRot = 30;
                }


            }

            if(other.gameObject.layer == 7)
            {
                yRot--;
                if (yRot <= -30)
                {
                    yRot = -30;
                }
            }

            
            
            
            

            

            if (Input.GetMouseButton(0))
            {
                MMVibrationManager.Haptic(HapticTypes.MediumImpact);
            }
        }*/
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("drift"))
        {
            drift = false;
            
        }
    }


   void DisableCar()
    {
        GetComponent<SplineFollower>().follow = false;
        GetComponent<SplineFollower>().followSpeed = 0f;
        transform.GetChild(1).transform.gameObject.SetActive(false);
        transform.GetChild(2).transform.gameObject.SetActive(false);
    }

    IEnumerator TransformCar()
    {
        yield return new WaitForSecondsRealtime(0.2f);

        if (crashed)
        {
            
            float timePassed = 0;
            while (timePassed < 1f)
            {
                timePassed += Time.deltaTime;
                GetComponent<SplineFollower>().direction = Spline.Direction.Backward;
                GetComponent<SplineFollower>().follow = true;
                GetComponent<SplineFollower>().followSpeed = 7;


                yield return null;
            }

            yield return new WaitForSecondsRealtime(0.4f);
            GetComponent<SplineFollower>().direction = Spline.Direction.Forward;
            
            GetComponent<SplineFollower>().follow = false;
            GetComponent<SplineFollower>().followSpeed = 0f;
            transform.GetChild(1).transform.gameObject.SetActive(true);
            transform.GetChild(2).transform.gameObject.SetActive(true);
            crashed = false;
            CancelInvoke("TurnOffMeshes");

            yield return new WaitForSecondsRealtime(0.5f);
            CancelInvoke("TurnOnMeshes");

        }

        

    }

    void FinishAnim()
    {
        Sequence seq = DOTween.Sequence();
        transform.DOLocalRotate(new Vector3(0, -240, 0), 0.5f, RotateMode.Fast).SetEase(Ease.OutQuad);
        transform.DOLocalMove(new Vector3(-800, 0, 980), 0.5f).SetEase(Ease.OutQuad);
        
        seq.Append(Camera.main.transform.DOLocalMove(new Vector3(-778.43f, 12.41f, 978f), 1f).SetEase(Ease.OutQuad));
        seq.Join(Camera.main.transform.DOLocalRotate(new Vector3(21.41f, -90f, 0), 0.5f, RotateMode.Fast).SetEase(Ease.OutQuad).OnComplete(()=>FinishMenu()));
    }

    void FinishMenu()
    {
        

        if(!AIController.Current.aiFinished)
        {
            LevelController.Current.WinGameOver();
        }else if( AIController.Current.aiFinished)
        {
            LevelController.Current.GameOver();
        }
        
    }

    IEnumerator RailRoadGates()
    {
        yield return new WaitForSecondsRealtime(6f);

        railRoadGate1.transform.DOLocalRotate(new Vector3(0, -90, 160), 1f, RotateMode.Fast).SetEase(Ease.Linear);
        railRoadGate2.transform.DOLocalRotate(new Vector3(0, -90, 160), 1f, RotateMode.Fast).SetEase(Ease.Linear);
    }

    void TurnOffMeshes()
    {
        transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        transform.GetChild(0).GetChild(1).GetComponent<MeshRenderer>().enabled = false;
        transform.GetChild(0).GetChild(2).GetComponent<MeshRenderer>().enabled = false;
        transform.GetChild(0).GetChild(3).GetComponent<MeshRenderer>().enabled = false;
        transform.GetChild(0).GetChild(4).GetComponent<MeshRenderer>().enabled = false;
    }

    void TurnOnMeshes()
    {
        transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        transform.GetChild(0).GetChild(1).GetComponent<MeshRenderer>().enabled = true;
        transform.GetChild(0).GetChild(2).GetComponent<MeshRenderer>().enabled = true;
        transform.GetChild(0).GetChild(3).GetComponent<MeshRenderer>().enabled = true;
        transform.GetChild(0).GetChild(4).GetComponent<MeshRenderer>().enabled = true;
    }

   IEnumerator RiseUpCar()
   {
        acceleration = false;
        yield return new WaitForSecondsRealtime(0.25f);

       
   }

    IEnumerator GetDownCar()
    {
        stop = false;

        yield return new WaitForSecondsRealtime(0.25f);
    }









}
