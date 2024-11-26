using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Object references
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private Transform playerTransform;
    
    [SerializeField] private float mouseSensitivity = 1.25f;

    private float xRotation;
    private float yRotation;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleInput();
        HandleRotation();
    }

    private void HandleInput()
    {
        // Establish mouse inputs on axis
        float xMouseInput = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        float yMouseInput = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        xRotation = xRotation - yMouseInput;
        yRotation = yRotation + xMouseInput;
    }

    private void HandleRotation()
    {
        // Limit the vertical rotation
        float xRotationLimit = 60f;
        xRotation = Mathf.Clamp(xRotation, -xRotationLimit, xRotationLimit);

        // Apply inputs to rotate the camera and player
        cameraPivot.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        playerTransform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}