using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GovForceFOV 
{
    private float sightDistance;
    private float fieldOfView;
    private LayerMask ignoreRayLayer;//all objects within IgnoreRaycast will be ignored
    private List<GameObject> visibleTargets;
    public GovForceFOV(float _fov,float _radius)
    {
        visibleTargets = new List<GameObject>();
        fieldOfView = _fov;
        sightDistance = _radius;
    }
    // Start is called before the first frame update
    public void Start()
    {
        ignoreRayLayer = 1 << LayerMask.NameToLayer("Ignore Raycast");
        ignoreRayLayer = ~ignoreRayLayer;
    }

    // Update is called once per frame
    public void Update(Transform _transform)
    {
        FieldOfVision(_transform);
    }
    /// <summary>
    /// Retrieves objects that are within field of view
    /// </summary>
    private void FieldOfVision(Transform _transform)
    {
        if (visibleTargets.Count > 0)
        {
            visibleTargets.Clear();
        }
        //Get objects within radius
        Collider[] objsInView = Physics.OverlapSphere(_transform.position, sightDistance, ignoreRayLayer);
        for (int i = 0; i < objsInView.Length; i++)
        {
            Vector3 dirToObj = (objsInView[i].transform.position - _transform.position).normalized;
            //Check if object is within field of view
            if (Vector3.Angle(_transform.forward, dirToObj) < fieldOfView / 2)
            {
                //Check if object is within sight radius
                if (Physics.Raycast(_transform.position, dirToObj, sightDistance, ignoreRayLayer))
                {
                    //add object within field of view and sight radius into list
                    visibleTargets.Add(objsInView[i].gameObject);
                }
            }
        }
    }
    #region Debug
    public void OnDrawGizmos(Transform _transform,bool _isDebugOn)
    {
        if (_isDebugOn)
        {
            Vector3 leftDir = (_transform.right * -1);
            Vector3 forwardDir = _transform.forward;
            Vector3 rightDir = _transform.right;
            //Left
            Debug.DrawLine(_transform.position, _transform.position + Vector3.Slerp(forwardDir, leftDir, (fieldOfView / 2) / 90) * sightDistance, Color.red);
            //Right
            Debug.DrawLine(_transform.position, _transform.position + Vector3.Slerp(forwardDir, rightDir, (fieldOfView / 2) / 90) * sightDistance, Color.red);
            if (visibleTargets.Count > 0)
            {
                for (int i = 0; i < visibleTargets.Count; i++)
                {
                    Debug.DrawLine(_transform.position, visibleTargets[i].transform.position, Color.cyan);
                }
            }
        }
    }
    #endregion
}
