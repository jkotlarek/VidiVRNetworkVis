using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ManipulateNetwork : MonoBehaviour {

    public TaskManager taskManager;
    public GameObject highlightedNodeObject;
    public GameObject altHighlightedNodeObject;

    //public Transform network;
    public Transform[] controllers;
    public VRTK_ControllerEvents[] controllerEvents;
    public Vector3[] start = { Vector3.zero, Vector3.zero };
    public Vector3[] current = { Vector3.zero, Vector3.zero };
    public bool[] active = { false, false, false };
    public bool[] nextStage = { false, false };
    public bool allowHighlight = true;

    public Vector3 startPos;
    public Quaternion startRot;
    public Vector3 startScale;
    public Vector3 startOffset;

    public float translateFactor = 1.0f;
    public float nodeScale = 1.0f;
    public float selectionThreshold;
    public List<Vector3> nodes;
    public Dictionary<int, GameObject> highlightedNodes;

    Transform highlightParent;

	// Use this for initialization
	void Start () {

        highlightedNodes = new Dictionary<int, GameObject>();

        if (nodes == null || nodes.Count == 0)
        {
            nodes = GetComponent<NetworkLoader>().nodePositions;
        }
    }

    // Update is called once per frame
    void Update () {
        if (active[0] && active[1])
        {
            if (!active[2])
            {
                active[2] = true;
                taskManager.IncremementTouchAction(1);
            }

            //Modify rotation
            /*
            Vector3 from = start[1] - start[0];
            Vector3 to = current[1] - current[0];
            Quaternion q = Quaternion.FromToRotation(from, to);
            
            transform.rotation = startRot * q;
            */

            //Modify scale
            float dist0 = Vector3.Distance(start[0], start[1]);
            float dist1 = Vector3.Distance(current[0], current[1]);

            transform.localScale = startScale * (dist1 / dist0);

            //Modify position
            Vector3 mid = (current[0] + current[1]) / 2f;
            transform.position = mid + startOffset * dist1 / dist0;
        }
    }

    public void SavePosition()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        startScale = transform.localScale;
        startOffset = transform.position - ((current[0] + current[1]) / 2);
    }

    public int WhichNode(Vector3 p)
    {
        Vector3 v = transform.InverseTransformPoint(p);

        Debug.Log("Orig: " + p);
        Debug.Log("Local: " + v);

        float minDist = float.MaxValue;
        for(int i = 0; i < nodes.Count; i++)
        {
            float dist = Vector3.Distance(v, nodes[i]);
            if (dist < minDist) minDist = dist;
            if (dist <= selectionThreshold)
            {
                Debug.Log("i: " + i);
                return i;
            }
        }

        Debug.Log("Min Dist: " + minDist);
        return -1;
    }

    /// <summary>
    /// Casts a ray from a point to select nodes in a cone.
    /// Returns the closest node to the ray origin if multiple are within the cone.
    /// Returns -1 if no nodes are inside the cone.
    /// </summary>
    /// <param name="ray">The ray in world space</param>
    /// <param name="angle">The angle selection threshold</param>
    /// <param name="maxDist">The maximum distance to cast</param>
    /// <returns>The index of the node hit.</returns>
    public int RayCastToNode(Ray ray, float angle, float maxDist)
    {
        ray.origin = transform.InverseTransformPoint(ray.origin);
        ray.direction = transform.InverseTransformDirection(ray.direction);

        float threshold = Mathf.Tan(angle);
        Dictionary<int, float> selectedNodes = new Dictionary<int, float>();

        for (int i = 0; i < nodes.Count; i++)
        {
            float dist = Vector3.Cross(ray.direction, nodes[i] - ray.origin).magnitude;
            float length = Vector3.Distance(ray.origin + ray.direction * Vector3.Dot(ray.direction, nodes[i] - ray.origin), ray.origin);
            if (dist <= Mathf.Max(length * threshold, nodeScale))
            {
                selectedNodes.Add(i, length);
            }

            
        }
        int node = -1;
        float l = float.MaxValue;
        foreach (KeyValuePair<int, float> kp in selectedNodes)
        {
            if (kp.Value < l)
            {
                node = kp.Key;
                l = kp.Value;
            }
        }

        return node;
    }

    public void ToggleHighlight(int index, bool alternateColor=false)
    {

        if (!alternateColor && taskManager.IsNodeProtected(index)) { return; }

        taskManager.IncrementHighlightAction(1);

        if (highlightParent == null)
        {
            var NodesGO = new GameObject("Highlighted Nodes");
            NodesGO.transform.SetParent(transform);
            NodesGO.transform.localPosition = Vector3.zero;
            NodesGO.transform.localRotation = Quaternion.identity;
            NodesGO.transform.localScale = Vector3.one;
            highlightParent = NodesGO.transform;
        }

        if (!highlightedNodes.ContainsKey(index))
        {
            var h = Instantiate(alternateColor ? altHighlightedNodeObject : highlightedNodeObject, highlightParent, false);
            h.transform.localPosition = nodes[index];
            h.transform.localScale *= nodeScale;
            h.name = "Node " + index;
            highlightedNodes.Add(index, h);
        }
        else
        {
            Destroy(highlightedNodes[index]);
            highlightedNodes.Remove(index);
        }
    }

    public void ClearAllHighlight()
    {
        foreach (var h in highlightedNodes)
        {
            Destroy(h.Value);
        }
        highlightedNodes.Clear();
    }
    
    public void Continue()
    {
        taskManager.NextStage();
    }


}
