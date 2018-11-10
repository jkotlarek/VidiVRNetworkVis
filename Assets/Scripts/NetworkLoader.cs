using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class NetworkLoader : MonoBehaviour {

    public string networkFolder;
    public string networkName;
    public GameObject nodeObject;
    public GameObject linkObject;
    public Transform nodeParent;
    public Transform linkParent;
    public bool optimizeMeshes = false;
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

            if (node.color != "" && !optimizeMeshes) {
                newNode.GetComponent<MeshRenderer>().material.SetColor("_Color", RGBStringToColor(node.color));
            }

            nodes.Add(newNode);
        }

        if (linkObject.GetComponent<LineRenderer>() != null)
        {
            links = InstantiateLinksLR(n.links);
        }
        else if (linkObject.GetComponent<MeshRenderer>() != null)
        {
            links = InstantiateLinksMesh(n.links);
        }

        if (optimizeMeshes)
        {
            OptimizeMeshes(nodeParent);

            if (linkObject.GetComponent<MeshRenderer>() != null)
            {
                OptimizeMeshes(linkParent);
            }
        }
    }

    void OptimizeMeshes(Transform parent)
    {
        MeshFilter[] filters = parent.GetComponentsInChildren<MeshFilter>();
        List<List<CombineInstance>> combiners = new List<List<CombineInstance>>();

        int verts = 0;

        foreach (var filter in filters)
        {
            verts += filter.sharedMesh.vertexCount;

            if (verts / 65534 >= combiners.Count)
            {
                combiners.Add(new List<CombineInstance>());
            }

            CombineInstance ci = new CombineInstance();
            ci.subMeshIndex = 0;
            ci.mesh = filter.sharedMesh;
            ci.transform = Matrix4x4.TRS(filter.transform.localPosition, filter.transform.localRotation, filter.transform.localScale);
            combiners[verts / 65534].Add(ci);

        }
        

        //Delete Children
        while (parent.childCount != 0)
        {
            DestroyImmediate(parent.GetChild(0).gameObject);
        }

        for (int i = 0; i < combiners.Count; i++)
        {
            GameObject submesh = new GameObject();
            submesh.name = "Node Mesh " + i;
            submesh.transform.parent = nodeParent;
            submesh.transform.localPosition = Vector3.zero;
            submesh.transform.localRotation = Quaternion.identity;
            submesh.transform.localScale = Vector3.one;

            Mesh mesh = new Mesh();
            MeshFilter meshfilter = submesh.AddComponent<MeshFilter>();
            mesh.CombineMeshes(combiners[i].ToArray());
            meshfilter.sharedMesh = mesh;

            MeshRenderer meshrenderer = submesh.AddComponent<MeshRenderer>();
            meshrenderer.sharedMaterial = nodeObject.GetComponent<MeshRenderer>().sharedMaterial;

        }
    }

    void ResizeArea(Vector3[] bounds)
    {

    }

    Color RGBStringToColor(string s)
    {
        float r = Convert.ToInt32(s.Substring(1, 2), 16) / 255f;
        float g = Convert.ToInt32(s.Substring(3, 2), 16) / 255f;
        float b = Convert.ToInt32(s.Substring(5, 2), 16) / 255f;

        return new Color(r, g, b);
    }

    //Instantiate Links with line renderer
    List<GameObject> InstantiateLinksLR(Link[] links)
    {
        List<GameObject> linkList = new List<GameObject>();
        foreach (Link l in links)
        {
            var link = Instantiate(linkObject, linkParent, false);
            link.name = "Link " + l.source + " to " + l.target;
            var lr = link.GetComponent<LineRenderer>();
            lr.SetPositions(new[]
            {
                nodes[l.source].transform.localPosition,
                nodes[l.target].transform.localPosition
            });
            linkList.Add(link);
        }
        return linkList;
    }

    //Instantiate Links with mesh (cube or cylinder)
    List<GameObject> InstantiateLinksMesh(Link[] links)
    {
        List<GameObject> linkList = new List<GameObject>();

        foreach(Link l in links)
        {
            var link = Instantiate(linkObject, linkParent, false);
            link.name = "Link " + l.source + " to " + l.target;

            var p1 = nodes[l.source].transform.localPosition;
            var p2 = nodes[l.target].transform.localPosition;

            link.transform.localPosition = (p1 + p2) / 2;
            link.transform.localRotation = Quaternion.FromToRotation(Vector3.up, p2 - p1);
            link.transform.localScale = new Vector3(link.transform.localScale.x, Vector3.Distance(p1, p2), link.transform.localScale.z);

            linkList.Add(link);
        }
        return linkList;
    }

}
