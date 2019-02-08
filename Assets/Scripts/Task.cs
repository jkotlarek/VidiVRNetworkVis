using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Task
{

    public string task;
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
    [NonSerialized] public Dataset dataset;

    public virtual void Init() { }
    public virtual void Begin() { }
    public virtual void End() { }
    
}

[Serializable]
public class Stage
{
    public float duration;
    public bool startInteraction;
    public bool endInteraction;
    public bool resetTransform;
    public View view;
    public string description;

    public Stage(float duration, bool startInteraction, bool endInteraction, bool resetTransform, View view, string description)
    {
        this.duration = duration;
        this.startInteraction = startInteraction;
        this.endInteraction = endInteraction;
        this.resetTransform = resetTransform;
        this.view = view;
        this.description = description;
    }
}

[Serializable]
public class Dataset
{
    public string name;
    public string filename;
    public float nodeSize;
    public float linkSize;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
}

public enum View
{
    BLANK, TITLE, NORMAL, PATH, RECALL, MUTATED
}

public class NodeHit
{
    public int node;
    public float distance;

    public NodeHit(int node, float distance)
    {
        this.node = node;
        this.distance = distance;
    }
}