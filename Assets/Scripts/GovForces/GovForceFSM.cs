using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GovForceStates { Patrol, Chase, Lasttknown, Shoot}
public class GovForceFSM : MonoBehaviour
{
    [HideInInspector]
    public GovForceStates States = GovForceStates.Patrol;
    public float sightDistance;
    public float fieldOfView = 45f;
    private LayerMask ignoreRayLayer;//all objects within IgnoreRaycast will be ignored
    private List<GameObject> visibleTargets = new List<GameObject>();
    public bool showDebug = false;
    // Start is called before the first frame update
    void Start()
    {
        ignoreRayLayer = 1 << LayerMask.NameToLayer("Ignore Raycast");
        ignoreRayLayer = ~ignoreRayLayer;
    }

    // Update is called once per frame
    void Update()
    {
        FieldOfVision();
        if (visibleTargets.Count > 0 && showDebug)
        {
            Debug.Log("Detected!");
        }
    }
    /// <summary>
    /// Retrieves objects that are within field of view
    /// </summary>
    private void FieldOfVision()
    {
        if (visibleTargets.Count > 0)
        {
            visibleTargets.Clear();
        }
        //Get objects within radius
        Collider[] objsInView = Physics.OverlapSphere(this.transform.position,sightDistance,ignoreRayLayer);
        for (int i = 0; i < objsInView.Length; i++)
        {
            Vector3 dirToObj = (objsInView[i].transform.position - this.transform.position).normalized;
            //Check if object is within field of view
            if (Vector3.Angle(this.transform.forward,dirToObj) < fieldOfView / 2)
            {
                //Check if object is within sight radius
                if (Physics.Raycast(this.transform.position,dirToObj,sightDistance,ignoreRayLayer))
                {
                    //add object within field of view and sight radius into list
                    visibleTargets.Add(objsInView[i].gameObject);
                }
            }
        }
    }
    #region Debug
    private void OnDrawGizmos()
    {
        if (showDebug)
        {
            //Sight Distance
            Debug.DrawLine(this.transform.position, this.transform.position + this.transform.forward * sightDistance, Color.red);
        }
        
    }
    #endregion
}
