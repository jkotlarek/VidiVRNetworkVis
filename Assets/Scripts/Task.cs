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

    public virtual void Init() { }
    public virtual void Begin() { }
    public virtual void End() { }
}
