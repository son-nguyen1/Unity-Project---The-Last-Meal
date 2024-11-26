using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellDoor : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.5f;

    // Position
    private Vector3 cellDoorPosition = new Vector3(-2.5f, 0f, 5.5f);
    private Quaternion cellDoorRotation = Quaternion.Euler(0f, 0f, 0f);

    public IEnumerator OpenCellDoor()
    {
        // Objects move and rotate like animation
        while (transform.position != cellDoorPosition && transform.rotation != cellDoorRotation)
        {
            transform.position = Vector3.Lerp(transform.position, cellDoorPosition, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, cellDoorRotation, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Snap to the exact position
        transform.position = cellDoorPosition;
        transform.rotation = cellDoorRotation;
    }
}