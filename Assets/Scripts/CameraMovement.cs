using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public float rotateAmount = 15.0f;
    public Transform player;

    public float lookAtCorrection;
    public Vector3 positionCorrection;

    private Rigidbody rb;

	// Use this for initialization
	void Start ()
    {
        //rotateAround = player.eulerAngles.y - 45f;
        transform.position = positionCorrection;
	}
	
	// Update is called once per frame
	void Update ()
    {
        OrbitCamera();
	}

    void OrbitCamera()
    {
        //Vector3 target = Vector3.zero;
        if (player == null)
            player = GameObject.FindObjectOfType<PlayerMovement>().gameObject.transform;

        Vector3 target = player.position;
        float x_rot = Input.GetAxis("Mouse Y") * rotateAmount;
        float y_rot = Input.GetAxis("Mouse X") * rotateAmount;
        RotateCamera(target, x_rot, y_rot);
    }

    void RotateCamera(Vector3 target, float x_rot, float y_rot)
    {
        Vector3 angles = transform.eulerAngles;
        angles.z = 0;
        transform.eulerAngles = angles;
        transform.RotateAround(target, Vector3.up, y_rot);
        transform.RotateAround(target, Vector3.left, x_rot);
        transform.LookAt(new Vector3(target.x, target.y + lookAtCorrection, target.z));
    }
}
