using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulsCamera : MonoBehaviour {


    public Camera cam;
    public Transform target;
    public float speedX = 360f;
    public float speedY = 240f;
    public float limitY = 40f;
    public float minDistance = 1.5f;
    public float hideDistance = 2f;
    public LayerMask obstacles;
    public LayerMask noPlayer;
    public float maxDistance;
    private Vector3 localPosition;
    private float currentYRotation;
    private LayerMask camOrigin;
    public float horizontal;

    public Transform reset;

    private Vector3 position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

	// Use this for initialization
	void Start ()
    {
        localPosition = target.InverseTransformPoint(position);
        maxDistance = Vector3.Distance(position, target.position);
        camOrigin = cam.cullingMask;
        //Cursor.lockState = CursorLockMode.Locked;
    }
	
	// Update is called once per frame
	void Update ()
    {
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.W))
            ResetPosition();
        else position = target.TransformPoint(localPosition);
        CameraRotation();
        ObstaclesRact();
        PlayerReact();
        localPosition = target.InverseTransformPoint(position);
    }

    void CameraRotation()
    {
        float mX = Input.GetAxis("Mouse X");
        float mY = -Input.GetAxis("Mouse Y");
        /* вращение вверх-вниз */
        if (mY != 0)
        {
            float temp = Mathf.Clamp(currentYRotation + mY * speedY * Time.deltaTime, -limitY, limitY);
            target.Rotate(0, mX * 5, 0);
            if (temp != currentYRotation)
            {
                float rot = temp - currentYRotation;
                transform.RotateAround(target.position, transform.right, rot);
                currentYRotation = temp;
            }
        }

        if (!GameObject.FindObjectOfType<PlayerMovement>().gameObject.GetComponent<PlayerMovement>().moving &&
            !GameObject.FindObjectOfType<PlayerMovement>().gameObject.GetComponent<PlayerMovement>().rifting)
        {
            /*вращение влево-вправо*/
            if (mX != 0)
            {
                transform.RotateAround(target.position, Vector3.up, mX * speedX * Time.deltaTime);
            }
        }

        transform.LookAt(target);
    }

    void ObstaclesRact()
    {
        float distance = Vector3.Distance(position, target.position);
        RaycastHit hit;
        if(Physics.Raycast(target.position, transform.position - target.position, out hit, maxDistance, obstacles))
        {
            position = hit.point;
        }
        else if(distance < maxDistance && !Physics.Raycast(position, -transform.forward, 1f, obstacles))
        {
            position -= transform.forward * 0.05f;
        }
    }

    void PlayerReact()
    {
        float distance = Vector3.Distance(position, target.position);
        if (distance < hideDistance)
            cam.cullingMask = noPlayer;
        else cam.cullingMask = camOrigin;
    }

    void ResetPosition()
    {
        transform.position = reset.position;
    }
}
