using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_3rdPersonCameraScript : MonoBehaviour
{
    private Transform camTransform;
    private Camera cam;

    [Header("Player Controller To Follow")]
    public PlayerController playerToFollow;
    private Transform playerTransform;

    [Header("Camera Properties")]
    public float distance = 6f; //distance between player & camera
    public float cameraHeight = 1f;
    public float yAngleMin = 0;
    public float yAngleMax = 50;
    public bool invertYAxis = false;
    public bool bLockCursor = true;

    [Header("Sensitivity")]
    public float sensitivityX = 4;
    public float sensitivityY = 4f;

    private float currentX = 0;
    private float currentY = 0;

    [HideInInspector] public bool canMoveCamera = true;

    // Start is called before the first frame update
    void Start()
    {
        camTransform = transform;
        cam = this.GetComponent<Camera>();
        playerTransform = playerToFollow.transform;
        canMoveCamera = true;

        if (bLockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void Update()
    {
        currentY = Mathf.Clamp(currentY, yAngleMin, yAngleMax);
        if (canMoveCamera) { GetInputs(); }
    }

    void LateUpdate()
    {
        if (canMoveCamera)
        {
            RotateCamera();
            
            SetCameraHeight();
        }
        else
        {
            ResetCamera();
            SetCameraHeight();
        }

    }

    private void SetCameraHeight()
    {
        cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y + cameraHeight, cam.transform.position.z);
    }

    private void RotateCamera()
    {
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        camTransform.position = playerTransform.position + rotation * direction;
        camTransform.LookAt(playerTransform.position);
    }

    private void ResetCamera()
    {
        currentX = 0; currentY = 0;
        Vector3 direction = new Vector3(0, 0, -distance);
        camTransform.position = playerTransform.position + direction;
        camTransform.LookAt(playerTransform.position);
    }

    private void GetInputs()
    {
        if (MouseInput.IsMovingMouse())
        {
            currentX += MouseInput.MouseX();

            if (invertYAxis) { currentY += MouseInput.MouseY(); }
            else { currentY -= MouseInput.MouseY(); }
        }

    }
}
