using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(HitmanFSM))]
public class HitmanFOVHandle : Editor
{
    HitmanFSM Hitman;
    private void OnEnable()
    {

        Hitman = (HitmanFSM)target;
    }
    private void OnSceneGUI()
    {
        if (Hitman.showDebug)
        {
            Handles.color = Color.blue;
            Handles.DrawWireArc(Hitman.transform.position, Vector3.up, Vector3.forward, 360, Hitman.sightDistance);
        }
    }
}
