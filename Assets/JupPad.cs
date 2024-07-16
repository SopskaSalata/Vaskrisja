using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JupPad : MonoBehaviour
{
    public float bounceForce;
    // Start is called before the first frame update

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        }  
    }
}
