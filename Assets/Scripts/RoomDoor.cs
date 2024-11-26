using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDoor : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.05f;

    // Position
    private Vector3 roomDoorPosition = new Vector3(-6.5f, 0f, 9.6f);
    private Quaternion roomDoorRotation = Quaternion.Euler(0f, 90f, 0f);

    public IEnumerator OpenRoomDoor()
    {
        // Objects move and rotate like animation
        while (transform.position != roomDoorPosition && transform.rotation != roomDoorRotation)
        {
            transform.position = Vector3.Lerp(transform.position, roomDoorPosition, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, roomDoorRotation, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Snap to the exact position
        transform.position = roomDoorPosition;
        transform.rotation = roomDoorRotation;
    }
}