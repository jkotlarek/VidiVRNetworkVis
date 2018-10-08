using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public enum Controller { Left, Right };

public class ManipulateNetwork : MonoBehaviour {

    public Transform network;
    public Transform controller0;
    public Transform controller1;
    public Vector3 start0;
    public Vector3 start1;
    public bool activeLeft;
    public bool activeRight;

	// Use this for initialization
	void Start () {
        controller0 = VRTK_DeviceFinder.GetControllerByIndex(0, true).transform;
        controller1 = VRTK_DeviceFinder.GetControllerByIndex(1, true).transform;
    }
	
	// Update is called once per frame
	void Update () {
		if (activeRight && activeLeft)
        {
            Vector3 current0 = controller0.position;
            Vector3 current1 = controller1.position;


        }
	}
}
