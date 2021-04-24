using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CharacterView : MonoBehaviour
{
    private PhotonView myPV;
    public float movemenSensity = 100f;

    public Transform player;

    private float XRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        myPV = transform.parent.gameObject.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myPV.IsMine)
        {
            float mouseX = Input.GetAxis("Mouse X") * movemenSensity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * movemenSensity * Time.deltaTime;

            XRotation -= mouseY;
            XRotation = Mathf.Clamp(XRotation, -90f, 44f);

            transform.localRotation = Quaternion.Euler(XRotation, 0f, 0f);
            player.Rotate(Vector3.up * mouseX);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.DrawRay(hit.point, hit.normal, Color.red, 1.25f);
    }
}
