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
    public Vector3 endPoint;
    public Vector3 finishPoint;
    public GameObject finish;
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
        

        if (other.CompareTag("TrainTrigger"))
        {
            if (!TrainSpawner.Current.trainSpawned)
            {
                TrainSpawner.Current.InstantiateTrain();
               
                
            }
            
        }

        if (other.CompareTag("Train"))
        {
            crashed = true;
            Debug.Log("anim");
            DisableCar();
            GetComponent<BoxCollider>().enabled = false;
            //InvokeRepeating("TurnOffMeshes", 0.2f, 0.2f);
            //InvokeRepeating("TurnOnMeshes", 0.25f, 0.25f);
            transform.DOScale(0, 0.1f).SetDelay(0.2f).SetEase(Ease.InOutSine).SetLoops(6, LoopType.Yoyo);
            MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
            StartCoroutine(TransformCar());

            //StartCoroutine(Crash());

        }

        if (other.CompareTag("FinishTrigger"))
        {
            playerFinished = true;
            GetComponent<SplineFollower>().enabled = false;
            Camera.main.GetComponent<BallCamera>().enabled = false;
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
            while (timePassed < 1.2f)
            {
                timePassed += Time.deltaTime;
                GetComponent<SplineFollower>().direction = Spline.Direction.Backward;
                GetComponent<SplineFollower>().follow = true;
                GetComponent<SplineFollower>().followSpeed = 10;


                yield return null;
            }

            yield return new WaitForSecondsRealtime(0.4f);
            GetComponent<SplineFollower>().direction = Spline.Direction.Forward;
            
            GetComponent<SplineFollower>().follow = false;
            GetComponent<SplineFollower>().followSpeed = 0f;
            transform.GetChild(1).transform.gameObject.SetActive(true);
            transform.GetChild(2).transform.gameObject.SetActive(true);
            GetComponent<BoxCollider>().enabled = true;
            crashed = false;

            //yield return new WaitForSecondsRealtime(0.4f);
            //GetComponent<BoxCollider>().enabled = true;
            //crashed = false;

            

        }

        

    }

    void FinishAnim()
    {
        Sequence seq = DOTween.Sequence();
        transform.DOLocalRotate(new Vector3(0, 120, 0), 0.5f, RotateMode.Fast).SetEase(Ease.OutQuad);
        transform.DOLocalMove(new Vector3((finish.transform.position.x - 62f), 0, (finish.transform.position.z - 9f)), 0.5f).SetEase(Ease.OutQuad);

        //seq.Append(Camera.main.transform.DOLocalMove(new Vector3(120f, 13f, 172.5f), 1f).SetEase(Ease.OutQuad));
        seq.Append(Camera.main.transform.DOLocalMove(new Vector3((finish.transform.position.x - 4.83f), (transform.position.y + 11.57f), (transform.position.z - 7.96f)), 1f).SetEase(Ease.OutQuad));
        seq.Join(Camera.main.transform.DOLocalRotate(new Vector3(11.6f, -90, 0f), 0.5f, RotateMode.Fast).SetEase(Ease.OutQuad).OnComplete(() => FinishMenu()));
        //seq.Join(Camera.main.transform.DOLocalRotate(new Vector3(11.6f, -90f, 0f), 0.5f, RotateMode.Fast).SetEase(Ease.OutQuad).OnComplete(()=>FinishMenu()));
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

   

    

   IEnumerator RiseUpCar()
   {
        acceleration = false;
        yield return new WaitForSecondsRealtime(0.25f);

       
   }

    









}
