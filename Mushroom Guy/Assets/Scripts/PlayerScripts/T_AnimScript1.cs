using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_AnimScript1 : MonoBehaviour
{
    public T_PlayerMovementScript TargetPlayerMovementScript;
    Animator A;
    

    // Start is called before the first frame update
    void Start()
    {
        A = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TargetPlayerMovementScript)
        {
            if (A)
            {
                
                A.SetFloat("Speed", TargetPlayerMovementScript.Speed);

                A.SetBool("Falling", !TargetPlayerMovementScript.CC.isGrounded);
            }
            
        }
    }
}
