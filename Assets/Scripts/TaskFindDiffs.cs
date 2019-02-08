using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindDiffsTask : Task
{
    int[][][] nodeLists = {
        new int[][] {
            new int[] { 16, 12, 30, 27 },
            new int[] { 0, 68, 74, 17 },
            new int[] { 230, 101, 9, 188 },
            new int[] { 426, 4078, 2414, 2543 }
        },
        new int[][] {
            new int[] { 16, 12, 30, 27 },
            new int[] { 0, 68, 74, 17 },
            new int[] { 230, 101, 9, 188 },
            new int[] { 1114, 1618, 2411, 3293 }
        }

    };

    public FindDiffsTask(string task, string viewcond, Dataset dataset)
    {
        this.task = task;
        this.viewcond = viewcond;
        this.dataset = dataset;
    }

    public override void Init()
    {
        Debug.Log("FindDiffsTask.Init");

        stages = new List<Stage>();
        stages.Add(new Stage(0, false, false, false, View.TITLE, "Dataset " + dataset.name.Substring(1,1) + "\nTask - Find Differences"));
        stages.Add(new Stage(30, false, false, true, View.MUTATED, ""));
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
