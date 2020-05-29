using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_PlayerMovementScript : MonoBehaviour
{
    private PlayerController controller;
    private Rigidbody rb;
    public CharacterController CC;

    float DistanceToTheGround = 0.0f;
    bool bGrounded = false;

    public float jumpHeight = 2f;
    public float JumpStrength = 20.0f;
    public float trampolineJumpHeight = 100f;
    public float GroundMoveSpeed = 0.1f;
    public float SprintModifier = 1.5f;
    public float SneakModifier = 0.5f;
    public float GroundDeceleration = 0.8f;
    public float AirMoveSpeed = 0.05f;
    public float AirDeceleration = 0.95f;
    public float AirControl = 0.1f;
    public float rotationRate = 90.0f;


    private Vector3 MyVelocity = new Vector3(0.0f, 0.0f, 0.0f);
    public float MaxVelocity = 0.16f;
    public float Speed = 0.0f;

    public bool movement2D = false;
    private bool canMove = true;
    private bool canJump = true;



    public PlayerRotationType RotationType = PlayerRotationType.Movement;

    private void Start()
    {
        
    }

    void Awake()
    {
        DistanceToTheGround = GetComponent<Collider>().bounds.extents.y;

        controller = this.GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        CC = GetComponent<CharacterController>();
        
    }

    void Update()
    {
        Jump();

        //UpdateMovement();
        UpdateRotation();
    }

    private void FixedUpdate()
    {
        CC.Move(MyVelocity);

        Vector3 MoveVector = T_GetMoveVector();

        float Y = MyVelocity.y;

        if (CC.isGrounded)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                MyVelocity += MoveVector * GroundMoveSpeed * SprintModifier;
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                MyVelocity += MoveVector * GroundMoveSpeed * SneakModifier;
            }
            else
            {
                MyVelocity += MoveVector * GroundMoveSpeed;
            }

            

            MyVelocity *= GroundDeceleration;

            if (MyVelocity.magnitude < 0.01)
            {
                MyVelocity = new Vector3(0.0f, 0.0f, 0.0f);

            }
            MyVelocity.y = 0.0f;
            MyVelocity.y -= 0.01f;
        }
        else
        {
            MyVelocity += (MoveVector * AirMoveSpeed);

            MyVelocity *= AirDeceleration;
            MyVelocity.y = Y;
            MyVelocity.y -= 0.01f;
        }

        Vector3 R = MyVelocity;
        R.y = 0.0f;


        Speed = R.magnitude;
        

        
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CC.isGrounded)
        {
            print("Jump");
            Vector3 JumpVector = MyVelocity;
            JumpVector.y = jumpHeight;

            MyVelocity.y = JumpVector.y;
        }

    }

    private void UpdateMovement()
    {
        Vector3 MoveVector = T_GetMoveVector();

        //transform.position += moveTranslation;
        //transform.Translate(MoveVector * movementSpeed * Time.deltaTime);
        //rb.AddForce(MoveVector * movementSpeed);
        //transform.position += (MoveVector * movementSpeed * Time.deltaTime);

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
                    Vector3 L = T_GetMoveVector();
                    L.y = 0.0f;

                    Quaternion NewRotation = Quaternion.LookRotation(L);
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

    public Vector3 T_GetMoveVector()
    {
        // Torben testing something
        // this code creates a moveTranslation vector based on the direction of the camera without rotating the character

        Vector3 NewMoveVector = new Vector3(0.0f, 0.0f, 0.0f);

        Vector3 Forward = Camera.main.transform.forward;
        Vector3 Right = Camera.main.transform.right;

        Forward.y = 0.0f;
        Right.y = 0.0f;

        Forward.Normalize();
        Right.Normalize();

        NewMoveVector = (Forward * controller.direction.y) + (Right * controller.direction.x);
        NewMoveVector.Normalize();

        return (NewMoveVector);
    }

    

    public void ToggleMovement(bool enabled)    //Call if movement ever needs to be stopped/started
    {
        canMove = enabled;
    }
}
