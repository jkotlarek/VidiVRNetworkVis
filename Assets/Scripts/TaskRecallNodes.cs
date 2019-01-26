using System;
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
    }

    public override void Begin()
    {
        taskStart = DateTime.Now;
    }

    public override void End()
    {
        taskEnd = DateTime.Now;
    }
}
