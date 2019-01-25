using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecallNodesTask : Task
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Init()
    {

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
