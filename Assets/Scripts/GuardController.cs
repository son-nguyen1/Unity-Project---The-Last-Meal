using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : MonoBehaviour
{
    // Object reference
    [SerializeField] private Transform doorPosition;

    [SerializeField] private float moveSpeed = 0.5f;

    private bool isWalking = false;
    private bool isDoorBanged = false;

    private void Update()
    {
        HandleInteraction();
    }

    private void FixedUpdate()
    {
        if (isDoorBanged)
        {
            HandleMovement();
        }
    }

    private void HandleInteraction()
    {
        // Retrieve data from detected objects
        float interactDistance = 0.75f;
        if (Physics.Raycast(transform.position, Vector3.right, out RaycastHit hit, interactDistance))
        {
            // Only object with this script
            if (hit.collider.TryGetComponent(out RoomDoor roomDoor))
            {
                StartCoroutine(roomDoor.OpenRoomDoor());
            }
        }
    }

    private void HandleMovement()
    {
        isWalking = true;

        // Move object to a position
        transform.position = Vector3.MoveTowards(transform.position, doorPosition.transform.position, moveSpeed * Time.deltaTime);

        if (transform.position == doorPosition.transform.position)
        {
            transform.position = doorPosition.transform.position;
            isWalking = false; // Walk animation
        }
    }

    public bool GuardIsWalking()
    {
        return isWalking;
    }

    public bool DoorIsBanging()
    {
        return isDoorBanged = true;
    }
}