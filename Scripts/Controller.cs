using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    public GameObject planetToOrbit;
    public Rigidbody2D rb;
    public Rigidbody2D planetRb;


    public float vinit = 0;
    public float timeToAccelerate = 100f;
    public float dt = 0.1f;
    public float time = 0.0f;
    Vector3 positionVector;
    public float omega = 0.0f;
    public  float alpha = 2f;
    //public float TangentAcceleration = vfinal - vinit / timeToAccelerate;
    public float TangentAcceleration = 0.1f;
    Vector3 vectorPlanetPos;
    private bool isInTrigger = false;

  
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        vectorPlanetPos = planetToOrbit.transform.position;
        InvokeRepeating("TimeToIncrement", 0.1f, dt);
        omega = vinit;
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
       

        //Rotate to custom pivot, seeking hinge joint or just his local axis in motion
        rb.angularVelocity = force;
    }

    void TimeToIncrement()
    {
        Debug.Log(rb.angularVelocity);
        if (isInTrigger)
        {
            while (time < timeToAccelerate)
            {
                time += dt;
                omega = (alpha * time * time);
                return;
            }
        }
        else
        {
            time = 0;
            //decelerate.
        }
        TangentAcceleration = 0f;

    }


    //Vector3 torqueForce = new Vector3(0, 0, torqueMag);
    //angle = Vector3.Angle(positionVector, yAxis) + STEP;
    //float torqueMag = force * mag * Mathf.Sin(angle); // the value of the torqueForce Mangnitudine


    void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            jobDisableRigidBodyOnExit(planetToOrbit);
        }
    }


    void jobSetRigidBodyToGameObject(GameObject planetToOrbitLocal)
    {
        planetToOrbit = planetToOrbitLocal;
        //Add Rigidbody2D to specific gameObject;
        if (planetToOrbitLocal.GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D planetRb;
            planetRb = planetToOrbitLocal.AddComponent<Rigidbody2D>();
            planetRb.gravityScale = 0;
            planetRb.bodyType = RigidbodyType2D.Static;
            //Add HingeJoint2D to specific gameObject;
            HingeJoint2D hingeJoint;
            hingeJoint = planetToOrbitLocal.AddComponent<HingeJoint2D>();
            hingeJoint.connectedBody = rb;
            isInTrigger = true;
        }
    }

    void jobDisableRigidBodyOnExit(GameObject planetToOrbitLocal)
    {
        //Diable rigidbody and hinge components from specific GameObject
        if (planetToOrbitLocal != null)
        {
            isInTrigger = false;
            Destroy(planetToOrbitLocal.GetComponent<HingeJoint2D>());
            Destroy(planetToOrbitLocal.GetComponent<Rigidbody2D>());
            Destroy(planetToOrbitLocal.GetComponent<Collider2D>());
        }
    }



    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Planet")
        {
            jobSetRigidBodyToGameObject(col.gameObject);

        }

    }
}

