using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public enum Devices { LeftController = 0, RightController = 1}

public class ControllerInput : MonoBehaviour {

    public ManipulateNetwork mnScript;
    public Devices device;

    VRTK_ControllerEvents controller;
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

        var col = GetComponentInChildren<SphereCollider>();
        if (col != null) hit = col.transform;

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
    }

    //Enable pointer
    void HandlePadTouchStart(object sender, ControllerInteractionEventArgs e)
    {

    }

    //Disable pointer
    void HandlePadTouchEnd(object sender, ControllerInteractionEventArgs e)
    {

    }

    //Select pointed object
    void HandlePadPressed(object sender, ControllerInteractionEventArgs e)
    {
        Debug.Log("pressed");
        if (hit == null)
        {
            hit = GetComponentInChildren<SphereCollider>().transform;
        }

        int indexOfNodeHit = mnScript.WhichNode(hit.position);
        if (indexOfNodeHit < 0)
        {
            return;
        }
        else
        {
            mnScript.ToggleHighlight(indexOfNodeHit);
        }

    }

    //nothing
    void HandlePadReleased(object sender, ControllerInteractionEventArgs e)
    {

    }

}
