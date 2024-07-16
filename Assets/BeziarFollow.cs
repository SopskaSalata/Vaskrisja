using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class BeziarFollow : MonoBehaviour
{
    [SerializeField]
    private Transform[] routs;

    public Collider2D Collider2D;

    private int routeToGo;

    private float tParam;

    private Vector2 playerPosition;

    private float speedModifier;

    private bool coroutineAllowed;

    public Rigidbody2D rg;
    // Start is called before the first frame update
    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        routeToGo = 0;
        tParam = 0; 
        speedModifier = 0.5f;
        coroutineAllowed = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       
        if (coroutineAllowed && other.gameObject.CompareTag("Gravity"))
        {
            
            StartCoroutine(GoByTheRoute(routeToGo));
        }

    }
    private IEnumerator GoByTheRoute(int routeNumber)
    {

        
        coroutineAllowed = false;

        Vector2 p0 = routs[routeNumber].GetChild(0).position;
        Vector2 p1 = routs[routeNumber].GetChild(1).position;
        Vector2 p2 = routs[routeNumber].GetChild(2).position;
        Vector2 p3 = routs[routeNumber].GetChild(3).position;

        while (tParam < 1)
        {
            tParam += Time.deltaTime * speedModifier;
            playerPosition = Mathf.Pow(1 - tParam, 3) * p0 +
                3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                MathF.Pow(tParam, 3) * p3;

            transform.position = playerPosition;

            rg.isKinematic = true;
            yield return new WaitForEndOfFrame();
        }

        tParam = 0f;
        routeToGo += 1;

        if(routeToGo > routs.Length -1)
           rg.isKinematic = false;

        coroutineAllowed |= true;
    }
}
