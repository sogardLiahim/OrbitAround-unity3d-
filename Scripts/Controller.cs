using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    public GameObject planetToOrbit;
    public Rigidbody2D rb;
    public Rigidbody2D planetRb;
    private const float TIMETOACCELERATE = Mathf.Infinity;
    private float dt = 0.1f;
    private float lastTimeWhenWasAccelerating = 0.0f;
    private float radiusOfPlanet = 0.1f;
    public float vinit = 1400f; 
    public float time = 0.0f;
    public float omega = 0.0f;
    public float alpha = 166f;
    public float TangentAcceleration = 0.1f;
    Vector3 vectorPlanetPos;
    Vector3 positionVector;
    public bool isInTrigger = false;
    private bool outsideAccelerationField = false;
  
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
       

        //Rotate to custom pivot, seeking hinge joint or just his local axis in motion
        rb.angularVelocity = force;
    }

    void TimeToIncrement()
    {
        Debug.Log("velocity : "+ rb.angularVelocity + " radius: " + radiusOfPlanet);
        if (isInTrigger)
        {
            //TODO custom acceleration for every planet, aplha is modified by position vector.
            while (time < TIMETOACCELERATE)/// this loop should execute only once.
            {
                time += dt;
                omega = (radiusOfPlanet * alpha * time) + vinit;
                outsideAccelerationField = false;
                return;
            }
        }

        else if (!outsideAccelerationField)
        {
            //record time, velocity for the last frame whose ball was still bound to planet
            vinit = omega;
            lastTimeWhenWasAccelerating = time;
            time = 0;
            outsideAccelerationField = true;
        }
        else
        {
            //negative acceleration
            omega -= Mathf.SmoothStep(0, omega, 0.4f);
        
        }
        TangentAcceleration = 0f;

        if (omega < 10f)
        {
            rb.AddForce(new Vector2(0, (-10f + omega)) * rb.mass);
        }

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
        radiusOfPlanet = planetToOrbitLocal.GetComponent<CircleCollider2D>().radius;
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
            Destroy(planetToOrbitLocal.GetComponent<HingeJoint2D>());
            Destroy(planetToOrbitLocal.GetComponent<Rigidbody2D>());
            Destroy(planetToOrbitLocal.GetComponent<Collider2D>());
            isInTrigger = false;
        }
    }

    //TODO jobGetEveryGameObject active.



    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Planet")
        {
            jobSetRigidBodyToGameObject(col.gameObject);

        }

    }

}

