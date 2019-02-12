using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecallNodesTask : Task
{
    int[][][] nodeLists = {
        new int[][] {
            new int[] { 5, 11, 15, 33, 8 },
            new int[] { 47, 8, 11, 49, 31 },
            new int[] { 188, 3, 117, 94, 53 },
            new int[] { 3975, 4378, 4935, 1393, 707 }
        },
        new int[][] {
            new int[] { 5, 11, 15, 33, 8 },
            new int[] { 47, 8, 11, 49, 31 },
            new int[] { 188, 3, 117, 94, 53 },
            new int[] { 3975, 4378, 4935, 1393, 707 }
        }
        
    };

    public RecallNodesTask(string task, string viewcond, Dataset dataset)
    {
        this.task = task;
        this.viewcond = viewcond;
        this.dataset = dataset;
    }

    public override void Init()
    {
        Debug.Log("RecallNodesTask.Init");

        stages = new List<Stage>();
        stages.Add(new Stage(0, false, false, false, View.TITLE, "Dataset " + dataset.name.Substring(1, 1) + "\nTask - Recall Nodes"));
        stages.Add(new Stage(30, false, false, true, View.RECALL, ""));
        stages.Add(new Stage(10, false, false, false, View.BLANK, "Please Wait"));
        stages.Add(new Stage(0, true, true, false, View.NORMAL, ""));

        int d = int.Parse(dataset.name.Substring(1, 1));
        int v = int.Parse(viewcond.Substring(0, 1)) - 2;
        correctNodes = nodeLists[v][d];
    }

    public override void Begin()
    {

    }

    public override void End()
    {

    }
}
