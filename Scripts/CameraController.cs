using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject ball;
    private Controller controllerBall;
    private Camera cam;
    private Vector3 positionVector;


    private float time = 0.0f;
    public float dt = 0.077f;
    
    
    // Use this for initialization
    void Start ()
    {
        cam = GetComponent<Camera>();
        controllerBall = ball.GetComponent<Controller>();

        cam.WorldToScreenPoint(ball.transform.position);
        transform.position = new Vector3(ball.transform.position.x, ball.transform.position.y, transform.position.z);
        InvokeRepeating("TimeToIncrement", 0.1f, Time.deltaTime);


    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {

        if (!controllerBall.isInTrigger)
        {
            Vector3 vectorPosInt = Vector3.Slerp(positionVector, Vector3.zero, time); //interpolate
            Debug.Log(positionVector);
            transform.position = new Vector3(ball.transform.position.x + vectorPosInt.x,
                                             ball.transform.position.y + vectorPosInt.y, transform.position.z);
           // transform.position = new Vector3(ball.transform.position.x, ball.transform.position.y, transform.position.z);
        }
        else // in raza cercului
        {
            positionVector = transform.position - ball.transform.position;
            time = 0;
        }

    }

    void InterpolateCameraWithOffset()
    {
    //TODO interpolateCameraWithOffset
    }

    void TimeToIncrement()
    {
            time += dt;

    }
}
