using UnityEngine;
using UnityEngine.UIElements;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float impact = 30f;

    public Camera cam;

    public ParticleSystem shot;

    public GameObject impactEffect;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
        
    }

    void Shoot()
    {
        shot.Play();
        
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

           GameObject impactPoint = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
           Destroy(impactPoint, 1f);
        }
    }
}
