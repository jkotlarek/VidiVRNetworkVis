using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindDiffsTask : Task
{
    int[][][] nodeLists = {
        new int[][] {
            new int[] { 6, 11, 12, 27, 32 },
            new int[] { 5, 16, 30, 41, 73 },
            new int[] { 72, 140, 188, 346, 360 },
            new int[] { 427, 1091, 1244, 4035, 4219 }
        },
        new int[][] {
            new int[] { 6, 11, 12, 27, 32 },
            new int[] { 5, 16, 30, 41, 73 },
            new int[] { 72, 140, 188, 346, 360 },
            new int[] { 427, 1091, 1244, 4035, 4219 }
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
        stages.Add(new Stage(1, false, false, false, View.BLANK, "Please Wait"));
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
