using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{

    public ManipulateNetwork mnScript;
    public ManipulateNetwork2D mnScript2D;
    public NetworkLoader nlScript;
    public Text timerText;
    public Text titleText;

    List<GameObject> network;

    public string viewCondition = "2D";
    public List<Task> tasks;
    public Dataset[] datasets;
    int i = 0;
    Stage stage;

    bool timerActive = false;
    float timeLeft = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        //Populate Task list
        foreach (Dataset d in datasets) tasks.Add(new ShortestPathTask("T1", viewCondition, d));
        foreach (Dataset d in datasets) tasks.Add(new RecallNodesTask("T2", viewCondition, d));
        foreach (Dataset d in datasets) tasks.Add(new FindDiffsTask("T3", viewCondition, d));
        
        //Start First task
        i = 0;
        InitTask();
        StartTask();
        NextStage();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimer(timeLeft);
            if (timeLeft <= 0f)
            {
                timerActive = false;
                if (mnScript != null) SetTimerVisibility(false);
                if (mnScript2D != null) timerText.text = "Continue";
                NextStage();
            }
        }
    }

    //Task object initialization
    public void InitTask()
    {
        Debug.Log("InitTask");
        tasks[i].Init();
    }

    //Taskwide configuration
    public void StartTask()
    {
        Debug.Log("StartTask");
        tasks[i].Begin();
    }

    //Stage specific configuration
    public void StartStage()
    {
        Debug.Log("StartStage: " + tasks[i].task + " (" + stage.view + ")");
        //Load Network
        //Highlight appropriate nodes
        //Enable or disable controls
        //View Control
        
        if (stage.startInteraction)
        {
            //Enable Controls
            if (mnScript != null) mnScript.allowHighlight = true;
            else if (mnScript2D != null) mnScript2D.allowHighlight = true;
            //Start timer
            tasks[i].taskStart = DateTime.Now;
        }

        switch (stage.view)
        {
            //Black screen
            case View.BLANK:
                UpdateTitle(stage.description);
                SetNetworkVisibility(false);
                SetTitleVisibility(true);
                break;

            //Graph with nodes removed
            case View.MUTATED:
                UpdateTitle(stage.description);
                LoadNetwork(tasks[i], tasks[i].correctNodes.ToList(), stage.resetTransform);
                SetNetworkVisibility(true);
                SetTitleVisibility(false);
                break;

            //Graph with no nodes removed or highlighted
            case View.NORMAL:
                UpdateTitle(stage.description);
                LoadNetwork(tasks[i], null, stage.resetTransform);
                SetNetworkVisibility(true);
                SetTitleVisibility(false);
                break;

            //Graph with first and last "correctNodes" highlighted
            case View.PATH:
                UpdateTitle(stage.description);
                LoadNetwork(tasks[i], null, stage.resetTransform);

                if(mnScript != null && tasks[i].correctNodes.Length > 1)
                {
                    mnScript.ToggleHighlight(tasks[i].correctNodes[0], true);
                    mnScript.ToggleHighlight(tasks[i].correctNodes[tasks[i].correctNodes.Length - 1], true);
                }
                else if (mnScript2D != null && tasks[i].correctNodes.Length > 1)
                {
                    mnScript2D.ToggleHighlight(tasks[i].correctNodes[0], true);
                    mnScript2D.ToggleHighlight(tasks[i].correctNodes[tasks[i].correctNodes.Length - 1], true);
                }

                SetNetworkVisibility(true);
                SetTitleVisibility(false);
                break;

            //Graph with all "correctNodes" highlighted
            case View.RECALL:
                UpdateTitle(stage.description);
                LoadNetwork(tasks[i], null, stage.resetTransform);
                
                foreach (int ii in tasks[i].correctNodes)
                {
                    if (mnScript != null) mnScript.ToggleHighlight(ii, true);
                    else if (mnScript2D != null) mnScript2D.ToggleHighlight(ii, true);
                }

                SetNetworkVisibility(true);
                SetTitleVisibility(false);
                break;

            //Black screen with task guide
            case View.TITLE:
                UpdateTitle(stage.description);
                SetNetworkVisibility(false);
                SetTitleVisibility(true);
                break;

        }

        //Start stage timer if there is one
        if (stage.duration > 0)
        {
            timeLeft = stage.duration;
            timerActive = true;
            SetTimerVisibility(true);
        }
    }

    public void EndStage()
    {
        if (stage != null)
        {
            if (stage.endInteraction)
            {
                //Disable Controls
                if (mnScript != null)
                {
                    mnScript.allowHighlight = false;
                    tasks[i].nodes = new int[mnScript.highlightedNodes.Count];
                    mnScript.highlightedNodes.Keys.ToArray().CopyTo(tasks[i].nodes, 0);
                    mnScript.ClearAllHighlight();
                }
                else if (mnScript2D != null)
                {
                    mnScript2D.allowHighlight = false;
                    tasks[i].nodes = new int[mnScript2D.highlightedNodes.Count];
                    mnScript2D.highlightedNodes.Keys.ToArray().CopyTo(tasks[i].nodes, 0);
                    mnScript2D.ClearAllHighlight();
                }
                //Stop timer
                tasks[i].taskEnd = DateTime.Now;
            }
            else
            {
                if (mnScript != null) mnScript.ClearAllHighlight();
                else if (mnScript2D != null) mnScript2D.ClearAllHighlight();
            }
        }
        stage = null;

    }

    //Complete Task
    //--Stop timers
    //--Do final calculations
    //--Output results
    public void EndTask()
    {
        tasks[i].End();

        tasks[i].time = (tasks[i].taskEnd - tasks[i].taskStart).TotalSeconds;
        
        var exc1 = tasks[i].nodes.Except(tasks[i].correctNodes);
        var exc2 = tasks[i].correctNodes.Except(tasks[i].nodes);
        int errors = exc1.Count() + exc2.Count();
        tasks[i].error = (double)errors / tasks[i].correctNodes.Length;

        tasks[i].totalInteractions = tasks[i].highlightActions + tasks[i].touchActions;

        if (!Directory.Exists(Application.streamingAssetsPath + "/out"))
            Directory.CreateDirectory(Application.streamingAssetsPath + "/out");

        string path = Application.streamingAssetsPath + "/out/" + tasks[i].viewcond + "_" + tasks[i].task + "_" + tasks[i].dataset.name + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".json";
        File.WriteAllText(path, JsonUtility.ToJson(tasks[i]));
    }

    //Transition to next task
    //--Load new scene
    //--Start Init for next task.
    public void TransitionNextTask()
    {
        Debug.Log("TransitionNextTask");
        EndTask();
        i++;

        if(i >= tasks.Count)
        {
            UpdateTitle("Done.");
            SetTitleVisibility(true);
            SetTimerVisibility(false);
            SetNetworkVisibility(false);
        }

        InitTask();
        StartTask();
        NextStage();
    }

    public void NextStage()
    {
        if(timerActive)
        {
            Debug.Log("NextStage prevented because timer is active");
            return;
        }
        else
        {
            Debug.Log("NextStage");
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

    }
    
    public void IncrementHighlightAction(int n)
    {
        tasks[i].highlightActions += n;
        Debug.Log("highlight");
    }

    public void IncremementTouchAction(int n)
    {
        tasks[i].touchActions += n;
        Debug.Log("touch");
    }

    public bool IsNodeProtected(int n)
    {
        if (stage.view == View.PATH && (tasks[i].correctNodes[0] == n || tasks[i].correctNodes[tasks[i].correctNodes.Length-1] == n))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void LoadNetwork(Task t, List<int> removedNodes, bool resetTransform)
    {
        Debug.Log("LoadNetwork");

        //Reset position and rotation
        if (resetTransform)
        {
            transform.position = t.dataset.position;
            transform.rotation = Quaternion.Euler(t.dataset.rotation);
            transform.localScale = t.dataset.scale;
        }

        //Set parameters and load network from file.
        nlScript.networkFolder = t.viewcond.ToLower();
        nlScript.networkName = t.dataset.filename;
        nlScript.nodeSize = t.dataset.nodeSize;
        nlScript.linkSize = t.dataset.linkSize;
        nlScript.LoadNetwork(removedNodes);
    }

    public void SetNetworkVisibility(bool vis)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(vis);
        }
    }

    public void UpdateTimer(float t)
    {
        timerText.text = ((int) t).ToString();
    }

    public void SetTimerVisibility(bool vis)
    {
        timerText.transform.parent.gameObject.SetActive(vis);
    }

    public void UpdateTitle(string s)
    {
        titleText.text = s;
    }

    public void SetTitleVisibility(bool vis)
    {
        titleText.transform.parent.gameObject.SetActive(vis);
    }

}
