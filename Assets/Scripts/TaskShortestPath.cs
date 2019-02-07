using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortestPathTask : Task
{
    int[][][] nodeLists = {
        new int[][] {
            new int[] { 4, 23 },
            new int[] { 20, 76 },
            new int[] { 139, 276 },
            new int[] { 800, 2634 }
        },
        new int[][] {
            new int[] { 1, 29 },
            new int[] { 22, 66 },
            new int[] { 238, 354 },
            new int[] { 3342, 4887 }
        }

    };

    public ShortestPathTask(string task, string viewcond, Dataset dataset)
    {
        this.task = task;
        this.viewcond = viewcond;
        this.dataset = dataset;
    }

    public override void Init()
    {
        Debug.Log("ShortestPathTask.Init");

        stages = new List<Stage>();
        stages.Add(new Stage(0, false, false, View.TITLE, "Dataset " + dataset.name.Substring(1, 1) + "\nTask - Shortest Path"));
        stages.Add(new Stage(0, true, true, View.PATH, ""));
        
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
