using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHealthBarFakeWater : MonoBehaviour {

    public GameObject waveBoneController;
    public GameObject waveBone;
    public float rotationSpeedX;
    public float rotationSpeedZ;
    public float limitRotationX;

    private bool achievePositiveX;
    private bool achieveNegativeX;

    // Use this for initialization
    void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        FakeWaterWave();
    }

    void FakeWaterWave()
    {
        //x rotation
        if(!achieveNegativeX)
        {
            waveBone.transform.Rotate(transform.right, 45 * Time.deltaTime * rotationSpeedX);
            waveBone.transform.Rotate(transform.up, 20 * Time.deltaTime * rotationSpeedZ);
            if (waveBone.transform.localRotation.eulerAngles.x >= limitRotationX)
            {
                achieveNegativeX = true;
                achievePositiveX = false;
            }
        }
        else if(!achievePositiveX)
        {
            waveBone.transform.Rotate(transform.right, 45 * Time.deltaTime * -rotationSpeedX);
            waveBone.transform.Rotate(transform.up, 20 * Time.deltaTime * -rotationSpeedZ);
            if (waveBone.transform.localRotation.eulerAngles.x >= limitRotationX)
            {
                achieveNegativeX = false;
                achievePositiveX = true;
            }
        }
    }
}
