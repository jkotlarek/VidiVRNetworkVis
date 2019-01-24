using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Task : MonoBehaviour
{

    public string task;
    public string dataset;

    public int interactions;
    public float time;
    public float error;

    public int[] nodes;

    [NonSerialized] public DateTime taskStart;
    [NonSerialized] public DateTime taskEnd;

    public virtual void Init() { }
    public virtual void Begin() { }
    public virtual void End() { }
}
