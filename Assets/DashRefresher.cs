using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashRefresher : MonoBehaviour
{
    private Player_Movement playermovement;
    private void Start()
    {
       playermovement = FindObjectOfType<Player_Movement>(); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playermovement.RefreshDash();
            Destroy(gameObject);
        }
    }
}
