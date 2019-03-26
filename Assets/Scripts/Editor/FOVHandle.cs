using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(GovForceFSM))]
public class FOVHandle : Editor
{
    GovForceFSM GovForce;
    private void OnEnable()
    {
       
        GovForce = (GovForceFSM)target;
    }
    private void OnSceneGUI()
    {
        if (GovForce.showDebug)
        {
            Handles.color = Color.blue;
            Handles.DrawWireArc(GovForce.transform.position, Vector3.up, Vector3.forward, 360, GovForce.sightDistance);
        }
    }
    
}
