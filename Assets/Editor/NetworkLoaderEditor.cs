using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NetworkLoader))]
public class NetworkLoaderEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        NetworkLoader script = (NetworkLoader)target;

        
        if (GUILayout.Button("Load"))
        {
            script.LoadNetwork();
        }
    }
}
