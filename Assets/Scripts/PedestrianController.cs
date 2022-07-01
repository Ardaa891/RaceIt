using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class PedestrianController : MonoBehaviour
{
    public static PedestrianController Current;

    public List<Collider> RagdollParts = new List<Collider>();
    public GameObject player;

    

    private void Awake()
    {
        Current = this;
        SetRagdollParts();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        Physics.gravity = new Vector3(0,-12,0);
        StartCoroutine(DisablePedestrian());
    }

   
    void Update()
    {
       
    }

    void SetRagdollParts()
    {
        Collider[] colliders = this.gameObject.GetComponentsInChildren<Collider>();

        foreach(Collider c in colliders)

            if(c.gameObject != this.gameObject)
            {
                c.isTrigger = true;
                c.GetComponent<Rigidbody>().useGravity = false;
                
                c.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                RagdollParts.Add(c);
            }
            
            
        
    }


    public void TurnOnRagdoll()
    {
        this.gameObject.GetComponent<Rigidbody>().useGravity = true;
        this.gameObject.GetComponent<Animator>().enabled = false;
        this.gameObject.GetComponent<Animator>().avatar = null;
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        //this.gameObject.GetComponent<BoxCollider>().isTrigger = false;
        foreach(Collider c in RagdollParts)
        {
            c.isTrigger = false;
            c.attachedRigidbody.useGravity = true;
            //c.attachedRigidbody.velocity = Vector3.zero;
            
            c.attachedRigidbody.AddForce(player.transform.forward * 5000);
        }

        StartCoroutine(KillPedestrian());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TurnOnRagdoll();
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);
            
        }
    }


    IEnumerator KillPedestrian()
    {
        yield return new WaitForSecondsRealtime(5f);

        gameObject.SetActive(false);
    }


    IEnumerator DisablePedestrian()
    {
       
            yield return new WaitForSecondsRealtime(25f);

            gameObject.SetActive(false);
        
        
    }
}
