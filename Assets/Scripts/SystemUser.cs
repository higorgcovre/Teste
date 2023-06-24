using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice;

public class SystemUser : MonoBehaviour
{
    public Rigidbody rb;
    void Start()
    {
        
    }
    void Update()
    {
        float rotation = Input.GetAxis("Horizontal");
        float speed = Input.GetAxis("Vertical");

        Quaternion rot = rb.rotation * Quaternion.Euler(0, rotation * Time.deltaTime * 60, 0);
        rb.MoveRotation(rot);
        Vector3 force = rot * Vector3.forward * speed * 1000 * Time.deltaTime;
        
        rb.AddForce(force);

        if(rb.velocity.magnitude > 2)
        {
            rb.velocity = rb.velocity.normalized * 2;
        }
    }
}
