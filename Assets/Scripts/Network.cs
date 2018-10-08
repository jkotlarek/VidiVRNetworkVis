using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Network
{
    public Link[] links;
    public GraphProperties graph_properties;
    public Node[] nodes;
}

[System.Serializable]
public class Link
{
    public int source;
    public int target;
}

[System.Serializable]
public class GraphProperties
{
    public string description;
    public string readme;
}

[System.Serializable]
public class Node
{
    public string label;
    public float x;
    public float y;
    public float z;
}