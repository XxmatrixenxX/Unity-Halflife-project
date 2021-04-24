using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Grapple : MonoBehaviour
{

    public float pullSpeed = 0.5f;
    public float stopDistance = 4F;
    public GameObject hookPrefab;
    public Transform shootTransform;
    
    private RopeGun hook;
    private bool pulling;
    private Rigidbody rigid;
    private PhotonView myPV;

    // Start is called before the first frame update
    void Start()
    {
        myPV = GetComponent<PhotonView>();
        rigid = GetComponent<Rigidbody>();
        pulling = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (myPV.IsMine)
        {
            if (hook == null && Input.GetMouseButtonDown(0))
            {
                pulling = false;
                hook = Instantiate(hookPrefab, shootTransform.position, Quaternion.identity).GetComponent<RopeGun>();
                hook.Initialize(this, shootTransform);
                StartCoroutine(DestoryHookAfterLifetime());
            }
            else if (hook != null && Input.GetMouseButtonDown(1))
            {
                DestroyHook();
            }

            if (!pulling || hook == null) return;

            if (Vector3.Distance(transform.position, hook.transform.position) <= stopDistance)
            {
                DestroyHook();
            }
            else
            {
                rigid.AddForce((hook.transform.position - transform.position).normalized * pullSpeed,
                    ForceMode.VelocityChange);
            }
        }
    }

    public void StartPull()
    {
        pulling = true;
    }

    private void DestroyHook()
    {
        if (hook == null) return;
        pulling = false;
        Destroy(hook.gameObject);
        hook = null;
    }

    private IEnumerator DestoryHookAfterLifetime()
    {
        yield return new WaitForSeconds(8f);
        DestroyHook();
    }
}
