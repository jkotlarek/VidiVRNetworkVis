using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortestPathTask : Task
{
    int[][][] nodeLists = {
        new int[][] {
            new int[] { 21, 0, 31, 24 },
            new int[] { 20, 24, 23, 68 },
            new int[] { 99, 4, 43, 65, 373 },
            new int[] { 2594, 2605, 2606, 2539, 2538 },
            new int[] { 391, 1292, 928, 1234, 952 }
        },
        new int[][] {
            new int[] { 17, 11, 24, 29 },
            new int[] { 20, 24, 23, 68 },
            new int[] { 99, 4, 43, 65, 373 },
            new int[] { 3280, 3231, 3184, 2178, 2277 },
            new int[] { 1307, 1794, 1718, 1119, 1141 }
        }

    };

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
        stages.Add(new Stage(0, false, false, View.TITLE, "Dataset " + dataset.Substring(1, 1) + "\nTask - Shortest Path"));
        stages.Add(new Stage(0, true, true, View.PATH, ""));
        
        int d = int.Parse(dataset.Substring(1, 1));
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
