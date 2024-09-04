using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    public Transform player; // Assign the player’s transform in the Inspector

    void Update()
    {
        if (player != null)
        {
            // Get the direction to the player
            Vector3 direction = player.position - transform.position;
            direction.Normalize(); // Normalize the direction vector

            // Calculate the angle between the object's forward direction and the direction to the player
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Rotate the object to face the player
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); // Subtract 90 degrees if your object is facing right by default
        }
    }
}
