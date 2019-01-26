using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class TaskManager : MonoBehaviour
{

    public ManipulateNetwork mnScript;

    public Task[] tasks;
    int i = 0;
    Stage stage;

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
        tasks[i].Init();
    }

    //Start task
    //--Enable user controls
    //--Start control sequences
    public void StartTask()
    {
        tasks[i].Begin();
    }

    public void StartStage()
    {

    }

    //Complete Task
    //--Stop timers
    //--Do final calculations
    //--Output results
    public void EndTask()
    {
        tasks[i].End();

        tasks[i].time = (tasks[i].taskEnd - tasks[i].taskStart).TotalSeconds;

        mnScript.highlightedNodes.Keys.CopyTo(tasks[i].nodes, 0);
        var exc1 = tasks[i].nodes.Except(tasks[i].correctNodes);
        var exc2 = tasks[i].correctNodes.Except(tasks[i].nodes);
        int errors = exc1.Count() + exc2.Count();
        tasks[i].error = (double)errors / tasks[i].nodes.Length;

        tasks[i].totalInteractions = tasks[i].highlightActions + tasks[i].touchActions;

        string path = Application.streamingAssetsPath + "/out/" + tasks[i].viewcond + "_" + tasks[i].name + "_" + tasks[i].dataset + ".json";
        File.WriteAllText(path, JsonUtility.ToJson(tasks[i]));
    }

    //Transition to next task
    //--Load new scene
    //--Start Init for next task.
    public void TransitionNextTask()
    {
        
    }

    public void NextStage()
    {
        if (tasks[i].stages.Count() == 0)
        {
            TransitionNextTask();
        }
        else
        {
            stage = tasks[i].stages[0];
            tasks[i].stages.RemoveAt(0);
            StartStage();
        }
    }

    public void IncrementHighlightAction()
    {
        //tasks[i].highlightActions++;
        Debug.Log("highlight");
    }

    public void IncremementTouchAction()
    {
        //tasks[i].touchActions++;
        Debug.Log("touch");
    }

}
