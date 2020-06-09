using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum T_PlayerState { Idle,Moving,Falling,Gliding}

public class T_PlayerMovementScript : MonoBehaviour
{
    private PlayerController controller;
    private Rigidbody rb;
    public CharacterController CC;

    float DistanceToTheGround = 0.0f;
    bool bGrounded = false;

    public float jumpHeight = 0.1f;
    public float JumpStrength = 0.5f;
    public float trampolineJumpHeight = 0.2f;
    public float GroundMoveSpeed = 0.05f;
    public float SprintModifier = 1.5f;
    public float SneakModifier = 0.5f;
    public float GroundDeceleration = 0.7f;
    public float AirMoveSpeed = 0.01f;
    public float AirDeceleration = 0.98f;
    //public float AirControl = 0.1f;
    public float Gravity = 0.01f;
    public float GravityScale = 1.0f;
    public float GlidingGravityScale = 0.2f;
    public float rotationRate = 90.0f;


    public Vector3 MyVelocity = new Vector3(0.0f, 0.0f, 0.0f);
    public float MinYVelocity = -0.5f;
    
    public float Speed = 0.0f;

    public bool movement2D = false;
    private bool canMove = true;
    private bool canJump = true;

    public T_PlayerState MyPlayerState = T_PlayerState.Idle;

    public PlayerRotationType RotationType = PlayerRotationType.Movement;

    private void Start()
    {
        
    }

    void Awake()
    {
        

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
        FixedUpdateMovement();

        
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (MyPlayerState == T_PlayerState.Idle || MyPlayerState == T_PlayerState.Moving)
            {
                print("Jump");
                Vector3 JumpVector = MyVelocity;
                JumpVector.y = jumpHeight;

                MyVelocity.y = JumpVector.y;
            }
            else if (MyPlayerState == T_PlayerState.Falling)
            {
                SwitchPlayerState(T_PlayerState.Gliding);
            }
            
        }

    }

    

    private void FixedUpdateMovement()
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
            MyVelocity.y -= (Gravity * GravityScale);
            
        }
        else
        {
            MyVelocity += (MoveVector * AirMoveSpeed);

            MyVelocity *= AirDeceleration;
            MyVelocity.y = Y;
            MyVelocity.y -= (Gravity * GravityScale);
        }

        if (MyVelocity.y < MinYVelocity * GravityScale)
        {
            MyVelocity.y = MinYVelocity * GravityScale;
        }

        Vector3 R = MyVelocity;
        R.y = 0.0f;

        Speed = R.magnitude;

        if (CC.isGrounded)
        {
            GravityScale = 1.0f;

            if (MyVelocity.magnitude > 0.01)
            {
                SwitchPlayerState(T_PlayerState.Moving);
            }
            else
            {
                SwitchPlayerState(T_PlayerState.Idle);
            }
        }
        else
        {
            if (MyPlayerState == T_PlayerState.Gliding)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    if (MyVelocity.y > 0.0f)
                    {
                        // rising 
                        GravityScale = 1.0f;
                    }
                    else
                    {
                        // falling
                        GravityScale = GlidingGravityScale;
                    }
                }
                else
                {
                    GravityScale = 1.0f;
                    SwitchPlayerState(T_PlayerState.Falling);
                }
            }
            else if (MyPlayerState != T_PlayerState.Falling)
            {
                GravityScale = 1.0f;
                SwitchPlayerState(T_PlayerState.Falling);
            }
            
        }
       


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
        // this code creates a move vector based on the direction of the camera without rotating the character

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

    private void SwitchPlayerState(T_PlayerState NewPlayerState)
    {
        MyPlayerState = NewPlayerState;

        switch (MyPlayerState)
        {
            case T_PlayerState.Idle:
                
                break;
            case T_PlayerState.Moving:
                break;
            case T_PlayerState.Falling:
                break;
            case T_PlayerState.Gliding:
                break;
        }
    }
}
