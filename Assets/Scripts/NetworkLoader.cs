using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class NetworkLoader : MonoBehaviour {

    public string networkFolder;
    public string networkName;
    public GameObject nodeObject;
    public GameObject linkObject;
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
        foreach (Node node in n.nodes)
        {
            var pos = new Vector3(node.x, node.y, node.z);
            nodes.Add(Instantiate(nodeObject, pos, Quaternion.identity, transform));
        }

        foreach (Link l in n.links)
        {
            var pos = nodes[l.source].transform.position;

            var link = Instantiate(linkObject, transform, false);
            var lr = link.GetComponent<LineRenderer>();
            lr.SetPositions(new[]
            {
                nodes[l.source].transform.position,
                nodes[l.target].transform.position
            });
        }
    }

    void ResizeArea(Vector3[] bounds)
    {

    }

}
