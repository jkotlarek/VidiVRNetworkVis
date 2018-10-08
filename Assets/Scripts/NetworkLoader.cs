using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class NetworkLoader : MonoBehaviour {

    public string networkFolder;
    public string networkName;
    public GameObject nodeObject;
    public GameObject linkObject;
    public Transform nodeParent;
    public Transform linkParent;
    public Vector3 dimensions;

    public List<GameObject> nodes;
    public List<GameObject> links;

	public void LoadNetwork()
    {
        Network n = readFile();
        var bounds = GetBounds(n.nodes);
        InstantiateObjects(n);

    }

    Network readFile()
    {
        string filename = Application.streamingAssetsPath + "/" + networkFolder + "/" + networkName;
        return JsonUtility.FromJson<Network>(File.ReadAllText(filename));
    }
    
    Vector3[] GetBounds(Node[] nodes)
    {
        Vector3[] bounds = { Vector3.positiveInfinity, Vector3.negativeInfinity };
        foreach (Node n in nodes)
        {
            Vector3 v = new Vector3(n.x, n.y, n.z);
            bounds[0] = Vector3.Min(v, bounds[0]);
            bounds[1] = Vector3.Max(v, bounds[1]);
        }

        return bounds;
    }

    void InstantiateObjects(Network n)
    {
        //Clean up existing children first
        while (transform.childCount != 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        if (nodeParent == null)
        {
            var NodesGO = new GameObject("Nodes");
            NodesGO.transform.SetParent(transform);
            NodesGO.transform.localPosition = Vector3.zero;
            nodeParent = NodesGO.transform;
        }
        if (linkParent == null)
        {
            var LinksGO = new GameObject("Links");
            LinksGO.transform.SetParent(transform);
            LinksGO.transform.localPosition = Vector3.zero;
            linkParent = LinksGO.transform;
        }

        nodes = new List<GameObject>();
        links = new List<GameObject>();

        foreach (Node node in n.nodes)
        {
            var pos = new Vector3(node.x, node.y, node.z);
            var newNode = Instantiate(nodeObject, nodeParent, false);
            newNode.transform.localPosition = pos;
            newNode.name = "Node " + nodes.Count;
            nodes.Add(newNode);
        }

        foreach (Link l in n.links)
        {
            var link = Instantiate(linkObject, linkParent, false);
            link.name = "Link " + l.source + " to " + l.target;
            var lr = link.GetComponent<LineRenderer>();
            lr.SetPositions(new[]
            {
                nodes[l.source].transform.localPosition,
                nodes[l.target].transform.localPosition
            });
            links.Add(link);
        }
    }

    void ResizeArea(Vector3[] bounds)
    {

    }

}
