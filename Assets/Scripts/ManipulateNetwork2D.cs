using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManipulateNetwork2D : MonoBehaviour
{

    public TaskManager taskManager;
    public GameObject highlightedNodeObject;
    public GameObject altHighlightedNodeObject;
    public GameObject tempHighlightedNodeObject;
    public GameObject highlightedLinkObject;
    
    public float nodeScale;
    public float minZoom;
    public float maxZoom;
    public float defaultZoom;
    public List<Vector3> nodes;
    public List<List<LinkObject>> links;
    public Dictionary<int, GameObject> highlightedNodes;
    public bool allowHighlight = true;

    Transform highlightParent;
    GameObject tempHighlight;
    Vector3 lastMousePos;
    List<GameObject> tempLinks;

    // Use this for initialization
    void Start()
    {

        highlightedNodes = new Dictionary<int, GameObject>();
        tempLinks = new List<GameObject>();

        if (nodes == null || nodes.Count == 0)
        {
            nodes = GetComponent<NetworkLoader>().nodePositions;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (allowHighlight && Input.GetMouseButtonUp(0))
        {
            Debug.Log("click");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int node = RayCastToNode(ray, nodeScale*1.5f, 100);
            if (node > -1)
            {
                taskManager.IncremementTouchAction(-1);
                ToggleHighlight(node);
            }
        }
        if (allowHighlight && !Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int node = RayCastToNode(ray, nodeScale * 1.5f, 100);
            TempHighlight(node);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float oldSize = Camera.main.orthographicSize;
        Vector3 oldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(0.5f, 0.5f, 0);
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - (scroll * 0.2f), minZoom, maxZoom);
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(0.5f, 0.5f, 0);
        Camera.main.transform.position -= mousePosition - oldMousePosition;
        if (scroll != 0) taskManager.IncremementTouchAction(1);


        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;
            taskManager.IncremementTouchAction(1);
        }
        //If left mouse is held down
        if (Input.GetMouseButton(0))
        {
            Camera.main.transform.position -= (Input.mousePosition - lastMousePos)*2*Camera.main.orthographicSize/Camera.main.pixelHeight;
            lastMousePos = Input.mousePosition;
        }

        //Clamp camera transform
        Vector3 v = Camera.main.transform.position;
        v.x = Mathf.Clamp(v.x, -1, 1);
        v.y = Mathf.Clamp(v.y, -1, 1);
        v.z = -10;
        Camera.main.transform.position = v;
    }

    public void ResetView()
    {
        Camera.main.transform.position.Set(0f, 0f, Camera.main.transform.position.z);
        Camera.main.orthographicSize = defaultZoom;
    }

    public int WhichNode(Vector3 p)
    {
        Vector3 v = transform.InverseTransformPoint(p);

        Debug.Log("Orig: " + p);
        Debug.Log("Local: " + v);

        float minDist = float.MaxValue;
        for (int i = 0; i < nodes.Count; i++)
        {
            float dist = Vector3.Distance(v, nodes[i]);
            if (dist < minDist) minDist = dist;
            if (dist <= nodeScale)
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
    public int RayCastToNode(Ray ray, float threshold, float maxDist)
    {
        ray.origin = transform.InverseTransformPoint(ray.origin);
        ray.direction = transform.InverseTransformDirection(ray.direction);
        
        Dictionary<int, float> selectedNodes = new Dictionary<int, float>();

        for (int i = 0; i < nodes.Count; i++)
        {
            float dist = Vector3.Cross(ray.direction, nodes[i] - ray.origin).magnitude;

            if (dist <= threshold)
            {
                selectedNodes.Add(i, dist);
            }
            
        }
        int node = -1;
        float d = float.MaxValue;
        foreach (KeyValuePair<int, float> kp in selectedNodes)
        {
            if (kp.Value < d)
            {
                node = kp.Key;
                d = kp.Value;
            }
        }

        return node;
    }

    public void ToggleHighlight(int index, bool alternateColor = false)
    {
        if(!alternateColor && taskManager.IsNodeProtected(index)) { return; }
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
            highlightedNodes.Add(index, h);

            if (tempHighlight != null)
            {
                Destroy(tempHighlight);
                tempHighlight = null;
            }
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

    public void TempHighlight(int index)
    {
        if (tempHighlight != null)
        {
            if (index == int.Parse(tempHighlight.name))
            {
                return;
            }
            else
            {
                Destroy(tempHighlight);
                tempHighlight = null;

                foreach (GameObject link in tempLinks) Destroy(link);
                tempLinks.Clear();
            }
        }

        if (index < 0)
        {
            foreach (GameObject link in tempLinks) Destroy(link);
            tempLinks.Clear();
            return;
        }

        if (!highlightedNodes.ContainsKey(index))
        {
            var h = Instantiate(tempHighlightedNodeObject, transform, false);
            h.transform.localPosition = nodes[index];
            h.transform.localScale *= nodeScale;
            h.name = index.ToString();
            tempHighlight = h;
        }
        
        foreach (LinkObject link in links[index])
        {
            var l = Instantiate(highlightedLinkObject, transform, false);
            l.transform.localPosition = link.position;
            l.transform.localRotation = link.rotation;
            l.transform.localScale = link.scale;
            tempLinks.Add(l);
        }
    }

    public void Continue()
    {
        taskManager.NextStage();
    }

}
