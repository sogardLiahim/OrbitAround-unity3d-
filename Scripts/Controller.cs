using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    public GameObject planetToOrbit;
    public Rigidbody2D rb;
    public Rigidbody2D planetRb;


    public float vinit = 0;
    public float vfinal = 40f;
    public float timeToAccelerate = 100f;
    public float dt = 1.1f;
    public float time = 0.0f;
    Vector3 positionVector;
    public float omega = 0.0f;
    public  float alpha = 2f;
    //public float TangentAcceleration = vfinal - vinit / timeToAccelerate;
    public float TangentAcceleration = 0.1f;
    Vector3 vectorPlanetPos;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        vectorPlanetPos = planetToOrbit.transform.position;
        InvokeRepeating("TimeToIncrement", 0.1f, 0.01f);
       // jobSetRigidBodyToGameObject();

    }

    // Update is called once per frame

    void FixedUpdate()
    {

     
     positionVector = planetToOrbit.transform.position - transform.position;

        Vector3 velocityThatIsPerpendicular = new Vector3(-positionVector.y, positionVector.x, 0);
        float mag = positionVector.magnitude;

        float force = rb.inertia * omega;
        Debug.Log(rb.inertia);


        //Rotate to custom pivot
        rb.angularVelocity = force;
    }

    void TimeToIncrement()
    {
        while (time < timeToAccelerate)
        {
            time += dt;
            omega = alpha * time * time;
            return;
        }
        TangentAcceleration = 0f;

    }

    void jobSetRigidBodyToGameObject(GameObject planetToOrbit)
    {
        Rigidbody2D planetRb;
        planetRb = planetToOrbit.AddComponent<Rigidbody2D>();
        planetRb.gravityScale = 0;
       //Add HingeJoint to specific gameObject;
        HingeJoint2D hingeJoint;
        hingeJoint = planetToOrbit.AddComponent<HingeJoint2D>();
        hingeJoint.connectedBody = rb;
    }

    void jobDisableRigidBodyOnExit(GameObject planetToOrbit)
    {
        Destroy(planetToOrbit.GetComponent<HingeJoint2D>());
        Destroy(planetToOrbit.GetComponent<Rigidbody2D>());
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        jobSetRigidBodyToGameObject(col.gameObject);
    }
    void OnTriggerExit2D(Collider2D col)
    {
        jobDisableRigidBodyOnExit(col.gameObject);
    }

    //Vector3 torqueForce = new Vector3(0, 0, torqueMag);
    //angle = Vector3.Angle(positionVector, yAxis) + STEP;
    //float torqueMag = force * mag * Mathf.Sin(angle); // the value of the torqueForce Mangnitudine


    void Update ()
    {
		
	}
}

