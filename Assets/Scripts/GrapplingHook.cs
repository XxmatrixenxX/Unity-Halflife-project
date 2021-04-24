using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
public class GrapplingHook : MonoBehaviour
{
    public float range = 100f;
    public Camera cam;
    
    //public GameObject GunTip;

    public GameObject pullingPoint;

    public GameObject Player;

    private bool thrown;

    private GameObject destroyMe;

    private bool hookCooldown;
    
    private PhotonView myPV;

    // Update is called once per frame

    void Start()
    {
        myPV = transform.parent.parent.parent.gameObject.GetComponent<PhotonView>();
    }

    void Update()
    {
        if (myPV.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !thrown && !hookCooldown)
            {
                destroyMe = ThrowingRope();
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                CutRope();
            }
        }
    }

   GameObject ThrowingRope()
    {
        thrown = true;
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range ))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("NotHookClimbable"))
            {
                
            }
            else
            {
                hookCooldown = true;
                Invoke("HookCoolDownReset", 1.75f);
                GameObject pullObject = PhotonNetwork.Instantiate(pullingPoint.name, hit.point, Quaternion.LookRotation(hit.normal));
                Vector3 objectPos = pullObject.transform.position;
                Player.GetComponent<EthanControls>().RopePulling(objectPos);
                return pullObject;
            }
        }

        return null;
    }

    public void CutRope()
    {
        if(destroyMe != null)
            PhotonNetwork.Destroy(destroyMe);
        Player.GetComponent<EthanControls>().StopPulling();
        thrown = false;
    }

    private void HookCoolDownReset()
    {
        hookCooldown = false;
    }
    
    
}
