using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfectCamera : MonoBehaviour {

    private float distanceAway;

    public float maxDistance = 2;
    public float minDistance = 1;

    public float distanceUp = -2;
    public float smooth = 4.0f;
    public float rotateAround = 70f;

    public Transform target;

    public LayerMask camOcclusion;

    RaycastHit hit;
    float cameraHeight = 55f;
    float cameraPan = 0f;
    float cameraRotateSpeed = 180f;
    Vector3 camPosition;
    Vector3 camMask;
    Vector3 followMask;

    private float horizontalAxis;
    private float verticalAxis;

	// Use this for initialization
	void Start ()
    {
        rotateAround = target.eulerAngles.y - 45f;
	}

    void LateUpdate()
    {
        horizontalAxis = Input.GetAxis("Mouse X");
        verticalAxis = Input.GetAxis("Mouse Y");

        Vector3 targetOffset = new Vector3(target.position.x, target.position.y + 2f, target.position.z);
        Quaternion rotation = Quaternion.Euler(cameraHeight, rotateAround, cameraPan);
        Vector3 vectorMask = Vector3.one;
        Vector3 rotateVector = rotation * vectorMask;
        camPosition = targetOffset + Vector3.up * distanceUp - rotateVector * distanceAway;
        camMask = targetOffset + Vector3.up * distanceUp - rotateVector * distanceAway;

        OccludeRay(ref targetOffset);
        SmoothCamMethod();

        transform.LookAt(target);

        if (rotateAround > 360)
            rotateAround = 0f;
        else if(rotateAround < 0f)
            rotateAround = rotateAround + 360f;

        rotateAround += horizontalAxis * cameraRotateSpeed * Time.deltaTime;
        distanceAway = Mathf.Clamp(distanceAway += verticalAxis, minDistance, maxDistance);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OccludeRay(ref Vector3 targetFollow)
    {
        RaycastHit wallHit = new RaycastHit();
        if(Physics.Linecast(targetFollow, camMask, out wallHit, camOcclusion))
        {
            smooth = 10f;
            camPosition = new Vector3(wallHit.point.x + wallHit.normal.x + 0.5f, camPosition.y, wallHit.point.z + wallHit.normal.z);
        }
    }

    void SmoothCamMethod()
    {
        smooth = 4f;
        transform.position = Vector3.Lerp(transform.position, camPosition, Time.deltaTime * smooth);
    }
}
