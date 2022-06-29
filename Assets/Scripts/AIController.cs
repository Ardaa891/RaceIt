using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using DG.Tweening;

public class AIController : MonoBehaviour
{
    public static AIController Current;

    public bool trafficHit;
    public bool drift;
    public bool aiFinished;

    public float yRot;

    public GameObject railRoadGate1, railRoadGate2;

    RaycastHit trafficCar;
    
    
    void Start()
    {
        Current = this;
        drift = false;
        
    }


    void Update()
    {
        if (LevelController.Current.gameActive)
        {
            GetComponent<SplineFollower>().followSpeed+=5;
            GetComponent<SplineFollower>().follow = true;
            transform.GetChild(0).GetChild(1).GetComponent<Animator>().SetBool("Holding", true);
            transform.GetChild(0).GetChild(2).GetComponent<Animator>().SetBool("Holding", true);
            transform.GetChild(0).GetChild(3).GetComponent<Animator>().SetBool("Holding", true);
            transform.GetChild(0).GetChild(4).GetComponent<Animator>().SetBool("Holding", true);

            if (GetComponent<SplineFollower>().followSpeed >= PlayerPrefs.GetInt("AIMaxSpeed"))
            {
                GetComponent<SplineFollower>().followSpeed = PlayerPrefs.GetInt("AIMaxSpeed");
            }


            if (GetComponent<SplineFollower>().followSpeed <= 0)
            {

                transform.GetChild(0).GetChild(1).GetComponent<Animator>().SetBool("Holding", false);
                transform.GetChild(0).GetChild(2).GetComponent<Animator>().SetBool("Holding", false);
                transform.GetChild(0).GetChild(3).GetComponent<Animator>().SetBool("Holding", false);
                transform.GetChild(0).GetChild(4).GetComponent<Animator>().SetBool("Holding", false);

            }

            if (Physics.Raycast(transform.position, transform.forward, out trafficCar, 100f))
            {
                if (trafficCar.collider.gameObject.CompareTag("Pedestrian"))
                {
                    trafficHit = true;
                    StartCoroutine(DecreaseSpeed());
                }
            }
            else
            {
                trafficHit = false;
                StartCoroutine(IncreaseSpeed());
            }



            if (Physics.Raycast(transform.position, transform.forward, out trafficCar, 30f))
            {
                if (trafficCar.collider.gameObject.CompareTag("TrafficCar"))
                {
                    trafficHit = true;
                    StartCoroutine(DecreaseSpeed());
                    Debug.Log("HIT");

                }

            }
            else
            {
                trafficHit = false;
                StartCoroutine(IncreaseSpeed());
            }

            if (Physics.Raycast(transform.position, transform.forward, out trafficCar, 75f))
            {
                if (trafficCar.collider.gameObject.CompareTag("Train"))
                {
                    Debug.Log("HIT");
                    trafficHit = true;
                    StartCoroutine(DecreaseSpeed());
                }
            }
            else
            {
                trafficHit = false;
                StartCoroutine(IncreaseSpeed());
            }



            if (!drift)
            {
                if (yRot > 0)
                {
                    yRot -= 2;

                    if (yRot <= 0)
                    {
                        yRot = 0;
                    }

                }
                else if (yRot < 0)
                {
                    yRot += 2;

                    if (yRot >= 0)
                    {
                        yRot = 0;
                    }
                }

                GetComponent<SplineFollower>().motion.rotationOffset = new Vector3(0, yRot, 0);




            }
        }
    }


        IEnumerator DecreaseSpeed()
        {
            while (trafficHit)
            {
                yield return new WaitForSecondsRealtime(0.001f);

                GetComponent<SplineFollower>().followSpeed -= 5f;
                if (GetComponent<SplineFollower>().followSpeed <= 0)
                {
                    GetComponent<SplineFollower>().followSpeed = 0;
                }
            }
        }

        IEnumerator IncreaseSpeed()
        {
            while (!trafficHit)
            {
                yield return new WaitForSecondsRealtime(0.001f);

                GetComponent<SplineFollower>().followSpeed += 5;

                if (GetComponent<SplineFollower>().followSpeed >= PlayerPrefs.GetInt("AIMaxSpeed"))
                {
                    GetComponent<SplineFollower>().followSpeed = PlayerPrefs.GetInt("AIMaxSpeed");
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            /*if (other.CompareTag("drift"))
            {
                if (other.CompareTag("drift"))
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

                    if (other.gameObject.layer == 7)
                    {
                        yRot--;
                        if (yRot <= -30)
                        {
                            yRot = -30;
                        }
                    }
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

        private void OnTriggerEnter(Collider other)
        {
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

            if (other.CompareTag("FinishTrigger"))
            {
                Debug.Log("AIFinished");

                    if (!EventController.Current.playerFinished)
                    {
                      aiFinished = true;
                    }



                
            }
        }


        IEnumerator RailRoadGates()
        {
            yield return new WaitForSecondsRealtime(6.5f);

            railRoadGate1.transform.DOLocalRotate(new Vector3(0, -90, 160), 1f, RotateMode.Fast).SetEase(Ease.Linear);
            railRoadGate2.transform.DOLocalRotate(new Vector3(0, -90, 9 - 160), 1f, RotateMode.Fast).SetEase(Ease.Linear);
        }
    } 
