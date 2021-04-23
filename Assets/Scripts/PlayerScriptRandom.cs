using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScriptRanmdom : MonoBehaviour
{
    public GameObject boxPrefab;
    public GameObject ballPrefab;
    public GameObject bombPrefab;

    public Transform hand;
    public Camera cam;
    public float throwForce;
    public LayerMask interactionMask;
    public float maxDistance;
    public float attractionForce = 1;
    public float grabDistance = 1;
    
    public bool onGround = true;
    public bool doubleJumped = false;
    public bool inAir = false;
    public bool dashed = false;
    public bool grapplingHook;
    public bool climbing;
    
    

    private GameObject objInHand;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Instantiate(boxPrefab, hand.position, Quaternion.identity); 
        }        
        if (Input.GetKeyDown(KeyCode.U))
        {
            GameObject ball = Instantiate(ballPrefab, hand.position, Quaternion.identity);
            ball.GetComponent<Rigidbody>().AddForce(cam.transform.forward*throwForce, ForceMode.Impulse);
        }
        
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (objInHand == null)
            {
                Ray ray = new Ray(cam.transform.position, cam.transform.forward);
                Debug.DrawLine(ray.origin, ray.GetPoint(maxDistance));
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, maxDistance, interactionMask))
                {
                    if (Vector3.Distance(hand.position, hit.transform.position) < grabDistance)
                    {
                        objInHand = hit.transform.gameObject;
                        objInHand.transform.position = hand.position;
                        objInHand.GetComponent<Rigidbody>().isKinematic = true;
                        objInHand.transform.parent = hand;
                    }
                }
                else
                {
                    Vector3 dir = Vector3.Normalize(hand.position - hit.transform.position);
                    hit.transform.GetComponent<Rigidbody>().AddForce(dir * attractionForce * Time.deltaTime, ForceMode.Impulse);
                    
                }
            }
           
        }
        


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (objInHand != null)
            {
                objInHand.GetComponent<Rigidbody>().isKinematic = false;
                objInHand.transform.parent = null;
                objInHand.GetComponent<Rigidbody>().AddForce(cam.transform.forward * throwForce, ForceMode.Impulse);
                objInHand = null;
            }
            else
            {
                GameObject bomb = Instantiate(bombPrefab, hand.position, Quaternion.identity);
                bomb.GetComponent<Rigidbody>().AddForce(cam.transform.forward * throwForce, ForceMode.Impulse);
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0.25f;
                
            }
            else
            {
                Time.timeScale = 1;
            }
            
        }

    }
}
