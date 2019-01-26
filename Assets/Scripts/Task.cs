using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Task : MonoBehaviour
{

    public string task;
    public string dataset;
    public string viewcond;

    public int highlightActions = 0;
    public int touchActions = 0;
    public int totalInteractions = 0;
    public double time;
    public double error;

    public int[] nodes;
    public int[] correctNodes;

    [NonSerialized] public DateTime taskStart;
    [NonSerialized] public DateTime taskEnd;
    [NonSerialized] public List<Stage> stages;

    public virtual void Init() { }
    public virtual void Begin() { }
    public virtual void End() { }
}

public class Stage
{
    public float duration;
    public bool startInteraction;
    public bool endInteraction;
    public View view;

    public Stage(float duration, bool startInteraction, bool endInteraction, View view)
    {
        this.duration = duration;
        this.startInteraction = startInteraction;
        this.endInteraction = endInteraction;
        this.view = view;
    }
}

public enum View
{
    BLANK, TITLE, NORMAL, PATH, RECALL, MUTATED
}