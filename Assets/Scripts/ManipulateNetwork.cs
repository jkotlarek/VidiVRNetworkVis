using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ManipulateNetwork : MonoBehaviour {

    //public Transform network;
    public Transform[] controllers;
    public VRTK_ControllerEvents[] controllerEvents;
    public Vector3[] start = { Vector3.zero, Vector3.zero };
    public Vector3[] current = { Vector3.zero, Vector3.zero };
    public bool[] active = {false, false};

    public Vector3 startPos;
    public Quaternion startRot;
    public Vector3 startScale;

	// Use this for initialization
	void Start () {

    }

    // Update is called once per frame
    void Update () {
        if (active[0] && active[1])
        {
            //Modify position
            Vector3 mid0 = (start[0] + start[1]) / 2;
            Vector3 mid1 = (current[0] + current[1]) / 2;

            transform.position = startPos + (mid1 - mid0);

            //Modify rotation
            Vector3 from = start[1] - start[0];
            Vector3 to = current[1] - current[0];
            Quaternion q = Quaternion.FromToRotation(from, to);

            transform.rotation = startRot * q;

            //Modify scale
            float dist0 = Vector3.Distance(start[0], start[1]);
            float dist1 = Vector3.Distance(current[0], current[1]);

            transform.localScale = startScale * (dist1 / dist0);
        }
	}

    public void SavePosition()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        startScale = transform.localScale;
    }
}
