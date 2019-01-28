﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecallNodesTask : Task
{
    public override void Init()
    {
        stages = new List<Stage>();
        stages.Add(new Stage(0, false, false, View.TITLE));
        stages.Add(new Stage(30, false, false, View.RECALL));
        stages.Add(new Stage(15, false, false, View.BLANK));
        stages.Add(new Stage(0, true, true, View.NORMAL));

        if (correctNodes.Length == 0)
        {
            switch (dataset)
            {
                case "D0":
                    correctNodes = new int[] { 0, 1, 2, 3, 4 };
                    break;
                case "D1":
                    correctNodes = new int[] { 0, 1, 2, 3, 4 };
                    break;
                case "D2":
                    correctNodes = new int[] { 0, 1, 2, 3, 4 };
                    break;
                case "D3":
                    correctNodes = new int[] { 0, 1, 2, 3, 4 };
                    break;
                case "D4":
                    correctNodes = new int[] { 0, 1, 2, 3, 4 };
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
