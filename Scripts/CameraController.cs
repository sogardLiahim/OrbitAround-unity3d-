using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject ball;
    public GameObject objectPrefab;
    public GameObject boundsCompositeObject;
    public LineRenderer lr;


    private Controller controllerBall;
    private Camera cam;
    private Vector3 positionVector;
    private  float time = .0f;
    private const float dt = .077f;
    
    
    // Use this for initialization
    void Start ()
    {
        cam = GetComponent<Camera>();
        controllerBall = ball.GetComponent<Controller>();

        cam.WorldToScreenPoint(ball.transform.position);
        transform.position = new Vector3(ball.transform.position.x, ball.transform.position.y, transform.position.z);
        InvokeRepeating("TimeToIncrement", 0.1f, Time.deltaTime);
        InvokeRepeating("InstantiateGameObject", 1f, 1f);
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;

      


    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        boundsCompositeObject.transform.position = new Vector3(boundsCompositeObject.transform.position.x, transform.position.y, 0);

        if (!controllerBall.isInTrigger)
        {
            Vector3 vectorPosInt = Vector3.Slerp(positionVector, Vector3.zero, time); //interpolate
            Debug.Log(positionVector);
            transform.position = new Vector3(ball.transform.position.x + vectorPosInt.x,
                                             ball.transform.position.y + vectorPosInt.y, transform.position.z);
            lr.SetPosition(0,new Vector3(transform.position.x,transform.position.y,0));
            lr.SetPosition(1, new Vector3(transform.position.x + vectorPosInt.x, transform.position.y + vectorPosInt.y, 0));
            lr.enabled = true;

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

        if (!controllerBall.isInTrigger)
        {
            time += dt;
            return;
        }

        return;
    }

    void InstantiateGameObject()
    {
        Vector3 desiredPositionToInstantiate = new Vector3(-1.2f, 1.16f, 0);
        Quaternion desiredRotationToInstatiate = new Quaternion(0, 0, 0, 1);
        GameObject cloneGameObject = Instantiate(objectPrefab, desiredPositionToInstantiate, desiredRotationToInstatiate);
        cloneGameObject.transform.localScale = cloneGameObject.transform.localScale * .5f;

    }


}
