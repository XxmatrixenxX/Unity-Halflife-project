using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UIElements;

public class Gun : MonoBehaviour
{
    private PhotonView myPV;
    public float damage = 10f;
    public float range = 100f;
    public float impact = 30f;

    public Camera cam;

    public ParticleSystem shot;

    public GameObject impactEffect;

    void Start()
    {
        myPV = transform.parent.parent.parent.gameObject.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myPV.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Shoot();
            }
        }

    }

    void Shoot()
    {
        GameObject effectDefGo = PhotonNetwork.Instantiate("GunTip", transform.GetChild(0).position,
            Quaternion.LookRotation(Vector3.forward), 0);
        Destroy(effectDefGo, 1f);
        //StartCoroutine("DestroyParticle",(effectDefGo,1));
        //shot.Play();

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            GunHit gunHit = hit.transform.GetComponent<GunHit>();
            if (gunHit != null)
            {
                gunHit.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impact);
            }

            GameObject impactPoint =
                PhotonNetwork.Instantiate(impactEffect.name, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactPoint, 1f);
            //StartCoroutine("DestroyParticle",(impactPoint, 1));
             
        }
    }

    /* IEnumerator DestroyParticle( GameObject destroyMe, float time)
     {
         yield return new WaitForSeconds(time);
         if(destroyMe != null)
                 PhotonNetwork.Destroy(destroyMe);
     }*/
    
    
}
