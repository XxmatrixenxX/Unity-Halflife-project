using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class MyPlayer : MonoBehaviourPun, IPunObservable
{
    public Camera camera;
    public GameObject healthBar;
    public float health = 1f;

    void UpdateHealthBar()
    {
        Vector3 oldScale = healthBar.transform.localScale;
        healthBar.transform.localScale = new Vector3(health, oldScale.y, oldScale.z);
    }

    public void Hit()
    {
        if (!photonView.IsMine)
            return;

        Debug.Log("is Hiiiiit");
        
        health -= 0.2f;
        if (health <= 0)
        {
            health = 0f;
            Debug.Log("Dead");
        }

        UpdateHealthBar();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            healthBar.layer = LayerMask.NameToLayer("Character");
        }
        else
        {
            camera.enabled = false;
            camera.gameObject.GetComponent<AudioListener>().enabled = false;
            gameObject.GetComponent<FirstPersonController>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthBar();
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                Hit();
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                gameObject.GetComponent<FirstPersonController>().enabled =
                    !gameObject.GetComponent<FirstPersonController>().enabled;
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
        }
        else
        {
            health = (float) stream.ReceiveNext();
            UpdateHealthBar();
        }
    }
}