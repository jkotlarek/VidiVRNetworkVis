﻿using System;
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
            new int[] { 3975, 4378, 4935, 1393, 707 },
            new int[] { 957, 1250, 307, 1381, 568 }
        },
        new int[][] {
            new int[] { 5, 11, 15, 33, 8 },
            new int[] { 47, 8, 11, 49, 31 },
            new int[] { 188, 3, 117, 94, 53 },
            new int[] { 3975, 4378, 4935, 1393, 707 },
            new int[] { 957, 1250, 307, 1381, 568 }
        }
        
    };

    public RecallNodesTask(string task, string dataset, string viewcond)
    {
        this.task = task;
        this.dataset = dataset;
        this.viewcond = viewcond;
    }

    public override void Init()
    {
        Debug.Log("RecallNodesTask.Init");

        stages = new List<Stage>();
        stages.Add(new Stage(0, false, false, View.TITLE, "Dataset " + dataset.Substring(1, 1) + "\nTask - Recall Nodes"));
        stages.Add(new Stage(30, false, false, View.RECALL, ""));
        stages.Add(new Stage(15, false, false, View.BLANK, "Please Wait"));
        stages.Add(new Stage(0, true, true, View.NORMAL, ""));

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
