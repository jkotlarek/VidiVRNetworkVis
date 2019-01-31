using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortestPathTask : Task
{
    public ShortestPathTask(string task, string dataset, string viewcond)
    {
        this.task = task;
        this.dataset = dataset;
        this.viewcond = viewcond;
    }

    public override void Init()
    {
        Debug.Log("ShortestPathTask.Init");

        stages = new List<Stage>();
        stages.Add(new Stage(0, false, false, View.TITLE, "Task - Shortest Path\nRead Handout"));
        stages.Add(new Stage(0, true, true, View.PATH, ""));

        if (correctNodes == null || correctNodes.Length == 0)
        {
            switch (dataset)
            {
                case "D0":
                    correctNodes = new int[] { 0, 1 };
                    break;
                case "D1":
                    correctNodes = new int[] { 0, 1 };
                    break;
                case "D2":
                    correctNodes = new int[] { 0, 1 };
                    break;
                case "D3":
                    correctNodes = new int[] { 0, 1 };
                    break;
                case "D4":
                    correctNodes = new int[] { 0, 1 };
                    break;
            }
        }
        
    }

    public override void Begin()
    {

    }

    public override void End()
    {

    }
}
