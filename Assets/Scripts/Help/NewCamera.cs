using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCamera : MonoBehaviour {

    public GameObject target;
    public float rotateSpeed = 5;
    private Vector3 offset;

	// Use this for initialization
	void Start ()
    {
        offset = target.transform.position - transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void LateUpdate()
    {
        RotateCamera();
    }

    void RotateCamera()
    {
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        float vertical = -Input.GetAxis("Mouse Y") * rotateSpeed;
        transform.RotateAround(target.transform.position, Vector3.up, vertical * Time.deltaTime);
        target.transform.Rotate(0, horizontal, 0);
        float desiredAngel = target.transform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(vertical, desiredAngel, 0);
        //transform.RotateAround(target.transform.position, Vector3.right, vertical * 240 * Time.deltaTime);
        transform.position = target.transform.position - (rotation * offset);
        //transform.RotateAround(target.transform.position, Vector3.right, vertical * 240 * Time.deltaTime);
        transform.LookAt(target.transform);
    }
}
