using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManipulateNetwork2D : MonoBehaviour
{

    public TaskManager taskManager;
    public GameObject highlightedNodeObject;
    
    public float nodeScale;
    public List<Vector3> nodes;
    public Dictionary<int, GameObject> highlightedNodes;

    Transform highlightParent;
    Vector3 lastMousePos;

    // Use this for initialization
    void Start()
    {

        highlightedNodes = new Dictionary<int, GameObject>();

        if (nodes == null || nodes.Count == 0)
        {
            nodes = GetComponent<NetworkLoader>().nodePositions;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("click");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int node = RayCastToNode(ray, nodeScale*1.5f, 100);
            if (node > -1) ToggleHighlight(node);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - (scroll * 0.2f), 0.01f, 0.7f);

        if (Input.GetMouseButtonDown(1))
        {
            lastMousePos = Input.mousePosition;
        }
        //If right mouse is held down
        if (Input.GetMouseButton(1))
        {
            Vector3 newPos = Camera.main.transform.position - (Input.mousePosition - lastMousePos)*2*Camera.main.orthographicSize/Camera.main.pixelHeight;
            newPos.x = Mathf.Clamp(newPos.x, -1, 1);
            newPos.y = Mathf.Clamp(newPos.y, -1, 1);
            newPos.z = Camera.main.transform.position.z; 
            Camera.main.transform.position = newPos;

            lastMousePos = Input.mousePosition;
        }
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
            if (kp.Value < float.MaxValue)
            {
                node = kp.Key;
                d = kp.Value;
            }
        }

        return node;
    }

    public void ToggleHighlight(int index)
    {

        //taskManager.IncrementHighlightAction();

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
            var h = Instantiate(highlightedNodeObject, highlightParent, false);
            h.transform.localPosition = nodes[index];
            h.transform.localScale *= nodeScale;
            highlightedNodes.Add(index, h);
        }
        else
        {
            Destroy(highlightedNodes[index]);
            highlightedNodes.Remove(index);
        }
    }


    public void Continue()
    {
        taskManager.NextStage();
    }


}
