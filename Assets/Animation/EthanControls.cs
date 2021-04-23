using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Crouch");
        }
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("CrouchPressed", true);
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("CrouchPressed",false);
        }
    }
}
