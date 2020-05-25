using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{

    public Transform playerTransform;
    public Transform cameraTransform;

    private Camera cam;

    private float distance = 5.0f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;

    private const float yMinimum = 10.0f;
    private const float yMaximum = 40.0f;


    private void Start()
    {
        cameraTransform = transform;
        cam = Camera.main;
    }

    private void Update()
    {
        currentX += Input.GetAxis("Mouse X");
        currentY += Input.GetAxis("Mouse Y");

        currentY = Mathf.Clamp(currentY, yMinimum, yMaximum);
    }

    private void LateUpdate()
    {
        Vector3 direction = new Vector3(0, 0, distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        cameraTransform.position = playerTransform.position + rotation * -direction;
        cameraTransform.LookAt(playerTransform.position);
    }

}
