using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BlackHole : MonoBehaviour
{

    public Transform player; // Player's transform
    public Rigidbody2D playerBody; // Player's Rigidbody2D component
    public float influenceRange; // Range within which the pull affects the player
    public float intensity; // Strength of the pulling force
    float distanceToPlayer; // Distance between the player and this object
    Vector2 pullForce; // Force vector to apply to the player

    void Start()
    {
        playerBody = player.GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component of the player
    }

    void Update()
    {
        distanceToPlayer = Vector2.Distance(player.position, transform.position);

        if (distanceToPlayer <= influenceRange && distanceToPlayer > 0) // Make sure distanceToPlayer is greater than 0 to avoid division by zero
        {
            Vector2 direction = (transform.position - player.position).normalized; // Calculate direction from player to this object

            // Calculate pull force based on direction, distance, and intensity
            pullForce = direction * intensity / distanceToPlayer;

            playerBody.AddForce(pullForce, ForceMode2D.Force); // Apply the force to the player

            playerBody.gravityScale = 0f;
        }

        else
        {
            playerBody.gravityScale = 2f;
        }
    }
}
