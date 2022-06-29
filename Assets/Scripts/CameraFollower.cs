using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform target;
    public float smootheSpeed = 0.125f;
    public Vector3 offset;
    
    
    void Start()
    {
        
    }

    
    void LateUpdate()
    {
        
        
        
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smootheSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }
}
