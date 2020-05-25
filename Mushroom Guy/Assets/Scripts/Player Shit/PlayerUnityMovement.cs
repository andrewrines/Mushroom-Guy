using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(PlayerController))]
public class PlayerUnityMovement : MonoBehaviour
{
    private PlayerController controller;
    private Rigidbody rb;

    public float jumpHeight = 2f;
    public float movementSpeed = 10f;

    private Vector3 moveTranslation;

    public bool movement2D = false;
    private bool canMove = true;
    private bool canJump = true;

    void Awake()
    {
        controller = this.GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();

    }

    void Update()
    {
        if (controller.MovementInput && canMove) 
        { 
            UpdateMovement();
        }
        if (canJump)
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("jump man");
            rb.AddForce(0, jumpHeight, 0, ForceMode.Impulse);
        }

    }

    private void UpdateMovement()
    {
        CreateMoveTranslationBasedOnPerspective();
        transform.position += moveTranslation;
        //rb.AddForce(moveTranslation);
    }

    private void CreateMoveTranslationBasedOnPerspective()  //If we ever decide to have a 2D movement section
    {
        if (movement2D) { Set2DMoveTranslation(); }
        else { Set3DMoveTranslation(); }
    }

    private void Set2DMoveTranslation() //Unneeded FOR NOW
    {
        moveTranslation = new Vector3(controller.direction.x, controller.direction.y, 0) * Time.deltaTime * movementSpeed;
    }

    public void Set3DMoveTranslation()
    {
        if (controller.direction.x > 0)
        {
            moveTranslation += transform.right;
        }
        else if (controller.direction.x < 0)
        {
            moveTranslation += -transform.right;
        }
        if (controller.direction.y > 0)
        {
            moveTranslation += transform.forward;
        }
        else if (controller.direction.y < 0)
        {
            moveTranslation += -transform.forward;
        }

        moveTranslation *= Time.deltaTime * movementSpeed;
    }

    public void ToggleMovement(bool enabled)    //Call if movement ever needs to be stopped/started
    {
        canMove = enabled;
    }
}
