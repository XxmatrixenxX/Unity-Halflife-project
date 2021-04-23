using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeGun : MonoBehaviour
{

    public float hookForce = 25f;
    private LineRenderer lr;
    private Rigidbody rigid;
    private Grapple grapple;


    public void Initialize(Grapple grapple, Transform shootTransform)
    {
        transform.forward = shootTransform.forward;
        this.grapple = grapple;
        rigid = GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();
        rigid.AddForce(transform.forward * hookForce, ForceMode.Impulse);
    }

    private void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if((LayerMask.GetMask("Default") & 1 <<other.gameObject.layer) > 0)
        {
            rigid.useGravity = false;
            rigid.isKinematic = true;

            grapple.StartPull();
        }
        
    }
}
