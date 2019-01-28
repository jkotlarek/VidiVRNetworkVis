using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class TaskManager : MonoBehaviour
{

    public ManipulateNetwork mnScript;
    public NetworkLoader nlScript;

    public Task[] tasks;
    int i = 0;
    Stage stage;

    bool timerActive = false;
    float timeLeft = 0.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0f)
            {
                NextStage();
                timerActive = false;
            }
        }
    }


    //Setup scene for task
    //--Load Network preset
    //--Show start screen
    public void InitTask()
    {
        tasks[i].Init();
    }

    //Task-wide configuration
    public void StartTask()
    {
        tasks[i].Begin();
    }

    //Stage specific configuration
    public void StartStage()
    {
        //Load Network
        //Highlight appropriate nodes
        //Enable or disable controls
        //View Control
        
        if (stage.startInteraction)
        {
            //Enable Controls
            mnScript.allowHighlight = true;
            //Start timer
            tasks[i].taskStart = DateTime.Now;
        }

        switch (stage.view)
        {
            //Black screen
            case View.BLANK:
                break;

            //Graph with nodes removed
            case View.MUTATED:
                break;

            //Graph with no nodes removed or highlighted
            case View.NORMAL:
                break;

            //Graph with first and last "correctNodes" highlighted
            case View.PATH:
                break;

            //Graph with all "correctNodes" highlighted
            case View.RECALL:
                break;

            //Black screen with task guide
            case View.TITLE:
                break;

        }

        //Start stage timer if there is one
        if (stage.duration > 0)
        {
            timeLeft = stage.duration;
            timerActive = true;
        }
    }

    public void EndStage()
    {
        if (stage.endInteraction)
        {
            //Disable Controls
            mnScript.allowHighlight = false;
            //Stop timer
            tasks[i].taskEnd = DateTime.Now;
        }
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

        string path = Application.streamingAssetsPath + "/out/" + tasks[i].viewcond + "_" + tasks[i].task + "_" + tasks[i].dataset + ".json";
        File.WriteAllText(path, JsonUtility.ToJson(tasks[i]));
    }

    //Transition to next task
    //--Load new scene
    //--Start Init for next task.
    public void TransitionNextTask()
    {
        EndTask();
        i++;
        InitTask();
        StartTask();
        NextStage();
    }

    public void NextStage()
    {
        EndStage();

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
        tasks[i].highlightActions++;
        Debug.Log("highlight");
    }

    public void IncremementTouchAction()
    {
        tasks[i].touchActions++;
        Debug.Log("touch");
    }

}
