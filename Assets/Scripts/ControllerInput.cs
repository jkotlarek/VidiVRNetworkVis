using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRTK;

public enum Devices { LeftController = 0, RightController = 1}

public class ControllerInput : MonoBehaviour {

    public ManipulateNetwork mnScript;
    public Devices device;

    public bool active = false;
    public float rayLength = 100f;
    public float rayAngle = 0f;

    VRTK_ControllerEvents controller;
    LineRenderer LR;
    Transform hit;

	// Use this for initialization
	void Start () {

        controller = GetComponent<VRTK_ControllerEvents>();

        controller.TriggerClicked += HandleTriggerClicked;
        controller.TriggerUnclicked += HandleTriggerUnclicked;
        controller.TouchpadTouchStart += HandlePadTouchStart;
        controller.TouchpadTouchEnd += HandlePadTouchEnd;
        controller.TouchpadPressed += HandlePadPressed;
        controller.TouchpadReleased += HandlePadReleased;
        controller.GripClicked += HandleGripClicked;
        controller.GripUnclicked += HandleGripUnclicked;


        var col = GetComponentInChildren<SphereCollider>();
        if (col != null) hit = col.transform;

        LR = GetComponent<LineRenderer>();

    }
	
	// Update is called once per frame
	void Update () {
		if (controller.triggerClicked)
        {
            mnScript.current[(int)device] = transform.position;
        }
	}

    void HandleTriggerClicked(object sender, ControllerInteractionEventArgs e)
    {
        mnScript.active[(int)device] = true;
        mnScript.start[(int)device] = transform.position;
        mnScript.current[(int)device] = transform.position;
        mnScript.SavePosition();
    }

    void HandleTriggerUnclicked(object sender, ControllerInteractionEventArgs e)
    {
        mnScript.active[(int)device] = false;
        mnScript.active[2] = false;
    }

    //Enable pointer
    void HandlePadTouchStart(object sender, ControllerInteractionEventArgs e)
    {
        if (mnScript.allowHighlight)
        {
            active = true;
            LR.enabled = true;
        }
    }

    //Disable pointer
    void HandlePadTouchEnd(object sender, ControllerInteractionEventArgs e)
    {
        if (mnScript.allowHighlight)
        {
            active = false;
            LR.enabled = false;
        }
    }

    //Select pointed object
    void HandlePadPressed(object sender, ControllerInteractionEventArgs e)
    {
        Debug.Log("pressed");
        if (active) RayCast();
    }

    //nothing
    void HandlePadReleased(object sender, ControllerInteractionEventArgs e)
    {

    }

    void HandleGripClicked(object sender, ControllerInteractionEventArgs e)
    {
        mnScript.nextStage[(int)device] = true;
        if (mnScript.nextStage[0] == true && mnScript.nextStage[1] == true)
        {
            mnScript.Continue();
        }
    }

    void HandleGripUnclicked(object sender, ControllerInteractionEventArgs e)
    {

        mnScript.nextStage[(int)device] = false;
    }

    void RayCast()
    {
        //Custom Angle-distance ray
        Ray ray = new Ray(transform.position, transform.forward);
        int indexOfNodeHit = mnScript.RayCastToNode(ray, Mathf.Deg2Rad * rayAngle, rayLength);

        if (indexOfNodeHit < 0) return;
        else mnScript.ToggleHighlight(indexOfNodeHit);
    }

}
