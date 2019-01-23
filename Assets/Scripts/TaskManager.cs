using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{

    DateTime taskStart;
    DateTime taskEnd;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //Setup scene for task
    //--Load Network preset
    //--Show start screen
    public void InitTask()
    {

    }

    //Start task
    //--Enable user controls
    //--Start control sequences
    public void StartTask()
    {
        taskStart = DateTime.Now;
    }

    //Complete Task
    //--Stop timers
    //--Do final calculations
    //--Output results
    public void EndTask()
    {
        taskEnd = DateTime.Now;


    }

    //Transition to next task
    //--Load new scene
    //--Start Init for next task.
    public void TransitionNextTask()
    {

    }

}
