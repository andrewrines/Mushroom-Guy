using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineTrigger : MonoBehaviour

{

    void OnCollisionEnter(Collision collision)

    {
        Debug.Log("Bounce Mushroom Man!");
        collision.gameObject.GetComponent<PlayerUnityMovement>().trampolineJump();

    }

}