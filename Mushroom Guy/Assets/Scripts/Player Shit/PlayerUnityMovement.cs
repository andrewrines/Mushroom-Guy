using UnityEngine;
using System.Collections;
using System;

public enum PlayerRotationType { Locked,Movement,Camera}

[RequireComponent(typeof(PlayerController))]
public class PlayerUnityMovement : MonoBehaviour
{
    private PlayerController controller;
    private Rigidbody rb;

    public float jumpHeight = 2f;
    public float trampolineJumpHeight = 100f;
    public float movementSpeed = 10f;
    public float rotationRate = 90.0f;

    private Vector3 moveTranslation;

    public bool movement2D = false;
    private bool canMove = true;
    private bool canJump = true;

    public PlayerRotationType RotationType = PlayerRotationType.Movement;

    void Awake()
    {
        controller = this.GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();

        // lock cursor //
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (controller.MovementInput && canMove) 
        { 
            UpdateMovement();
            UpdateRotation();
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

    private void UpdateRotation()
    {
        // this rotates the player 
        switch (RotationType)
        {
            case PlayerRotationType.Movement:
                // movement rotates towards the movement direction
                // if receiving movement input only
                if (controller.MovementInput)
                {
                    Quaternion NewRotation = Quaternion.LookRotation(moveTranslation);
                    NewRotation = Quaternion.Slerp(transform.rotation, NewRotation, Time.deltaTime * rotationRate);

                    transform.rotation = NewRotation;
                }
                break;
            case PlayerRotationType.Camera:
                // rotates towards the direction the camera is facing
                if (controller.MovementInput)
                {
                    Vector3 L = transform.position - Camera.main.transform.position;
                    L.y = 0.0f;

                    Quaternion NewRotation = Quaternion.LookRotation(L);
                    NewRotation = Quaternion.Slerp(transform.rotation, NewRotation, Time.deltaTime * rotationRate);

                    transform.rotation = NewRotation;
                }
                break;
            case PlayerRotationType.Locked:
                // locks rotation
                break;
       
        }
    }

    private void CreateMoveTranslationBasedOnPerspective()  //If we ever decide to have a 2D movement section
    {
        // torben testing something
        T_Set3DMoveTranslation();

        
        /*
        if (movement2D) { Set2DMoveTranslation(); }
        else { Set3DMoveTranslation(); }
        */
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

    public void T_Set3DMoveTranslation()
    {
        // Torben testing something
        // this code creates a moveTranslation vector based on the direction of the camera without rotating the character
        

        Vector3 Forward = Camera.main.transform.forward;
        Vector3 Right = Camera.main.transform.right;

        Forward.y = 0.0f;
        Right.y = 0.0f;

        Forward.Normalize();
        Right.Normalize();

        moveTranslation = (Forward * controller.direction.y) + (Right * controller.direction.x);
        moveTranslation.Normalize();
        moveTranslation *= Time.deltaTime * movementSpeed;
    }

    public void ToggleMovement(bool enabled)    //Call if movement ever needs to be stopped/started
    {
        canMove = enabled;
    }

    //Ed Trampoline Jump Function

    public void trampolineJump ()
    {
        rb.AddForce(0, trampolineJumpHeight, 0, ForceMode.Impulse);
    }
}
