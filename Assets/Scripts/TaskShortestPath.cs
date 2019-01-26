using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortestPathTask : Task
{
    public override void Init()
    {
        stages = new List<Stage>();
        stages.Add(new Stage(0, false, false, View.TITLE));
        stages.Add(new Stage(0, true, true, View.PATH));
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
