using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class TaskManager : MonoBehaviour
{

    public ManipulateNetwork mnScript;

    Task[] tasks;
    Task task;

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
        task.Init();
    }

    //Start task
    //--Enable user controls
    //--Start control sequences
    public void StartTask()
    {
        task.Begin();
    }

    //Complete Task
    //--Stop timers
    //--Do final calculations
    //--Output results
    public void EndTask()
    {
        task.End();

        task.time = (task.taskEnd - task.taskStart).TotalSeconds;

        mnScript.highlightedNodes.Keys.CopyTo(task.nodes, 0);
        var exc1 = task.nodes.Except(task.correctNodes);
        var exc2 = task.correctNodes.Except(task.nodes);
        int errors = exc1.Count() + exc2.Count();

        task.error = (double)errors / task.nodes.Length;

        string path = Application.streamingAssetsPath + "/out/" + task.viewcond + "_" + task.name + "_" + task.dataset + ".json";
        File.WriteAllText(path, JsonUtility.ToJson(task));
    }

    //Transition to next task
    //--Load new scene
    //--Start Init for next task.
    public void TransitionNextTask()
    {
        
    }

    public void IncrementHighlightAction()
    {
        task.highlightActions++;
    }

    public void IncremementTouchAction()
    {
        task.touchActions++;
    }

}
