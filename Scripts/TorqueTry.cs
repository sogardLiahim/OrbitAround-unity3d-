using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorqueTry : MonoBehaviour {

    // Use this for initialization
    public GameObject planetToOrbit;
    public Rigidbody2D rb;

    float speed = 30.0f;
    float rotationSpeed = 10.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void fixedUpdate()
    {
        // Multiply speed with the input.
        // var speed can be changed in the inspector
        // without changing any scripts
        Vector3 force = new Vector3(0, 0, 100f);
        force = speed * force;
        // Don’t multiply by Time.deltaTime
        // since forces are already time independent.
        // Apply force along the z axis of the object
        rb.AddRelativeForce(force);
        Vector3 torque = new Vector3(Input.GetAxis("Horizontal"), 0, 0); 
        torque = rotationSpeed * torque;
        // Rotate around the world y-axis
        rb.AddTorque(torque.magnitude);

    }

	// Update is called once per frame
	void Update () {
		
	}
}
