using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ControllerGrabNetwork : MonoBehaviour {

    public ManipulateNetwork mnScript;
    public Controller whichController;

    VRTK_ControllerEvents controller;


    // Use this for initialization
    void Start () {
        controller = GetComponent<VRTK_ControllerEvents>();

        controller.TriggerUnclicked += HandleTriggerUnclicked;
        controller.TriggerClicked += HandleTriggerClicked;
	}
	
	void HandleTriggerUnclicked(object sender, ControllerInteractionEventArgs e)
    {
        if (whichController == Controller.Left)
        {
            mnScript.activeLeft = true;
        }
        else
        {
            mnScript.activeRight = true;
        }
    }

    void HandleTriggerClicked(object sender, ControllerInteractionEventArgs e)
    {
        if (whichController == Controller.Left)
        {
            mnScript.activeLeft = false;
        }
        else
        {
            mnScript.activeRight = false;
        }
    }


}
