using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    private Rigidbody rbball;
    private Vector3 direction = Vector3.one;
    [SerializeField] private float force;
    // Start is called before the first frame update
    private int points1;
    private int points2;
    void Start()
    {
        // rbball = transform.GetComponent<Rigidbody>();
        // direction = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-10.0f, 10.0f), 0);
        // rbball.AddForce(direction * force);
        // points1 = 0;
        // points2 = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Collider>().CompareTag("Limit1"))
        {
            points1++;            
            print("Red Scores!");
            print("Points: " + points1 + "-" + points2);
            transform.position = Vector3.zero;
        }
        //if (other.collider.CompareTag("Limit2"))
        if (other.GetComponent<Collider>().CompareTag("Limit2"))
        {
            points2++;
            print("Blue Scores!");
            print("Points: " + points1 + "-" + points2);
            transform.position = Vector3.zero;

        }
    }

    // private void OnCollisionEnter(Collision other) {

    //     if (other.collider.CompareTag("Limit1"))
    //     {
    //         points1++;            
    //         print("Red Scores!");
    //         print("Points: " + points1 + "-" + points2);
    //     }
    //     //if (other.collider.CompareTag("Limit2"))
    //     if (other.collider.CompareTag("Limit2"))
    //     {
    //         points2++;
    //         print("Blue Scores!");
    //         print("Points: " + points1 + "-" + points2);
    //     }        
    // }
}
