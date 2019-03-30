using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitmanFSM : MonoBehaviour
{
    [Header("Field Of Vision")]
    public float sightDistance;
    [Range(0, 180)]
    public float fieldOfView = 45f;

    [Header("Flee State")]
    [Tooltip("Distance to avoid obstacles")]
    public float avoidDistance = 5;
    [Tooltip("Amount of smoke grenades hitman has")]
    public int smokegrenades = 1;

    [Header("Assassinate State")]
    private GameObject contractTarget;
    
    public bool showDebug = false;
    private FieldOfView hitmanFOV;
    // Start is called before the first frame update
    void Start()
    {
        if (hitmanFOV == null)
        {
            hitmanFOV = new FieldOfView(fieldOfView, sightDistance);
        }
        hitmanFOV.Start();
    }

    // Update is called once per frame
    void Update()
    {
        //[TODO:] Delete this when done testing vision
        GameObject visibleTarget = GetFirstVisibleObject(Aspect.AspectName.Pacifist);
    }
    /// <summary>
    /// Get All visible objects not in the ignore raycast layer
    /// </summary>
    /// <returns>returns a list of all objects visible</returns>
    public List<GameObject> GetVisibleObjects()
    {
        return hitmanFOV.GetAllVisibleObjects(this.transform);
    }
    //[TODO:] delete this function when done
    public GameObject GetFirstVisibleObject(params Aspect.AspectName[] _aspectNames)
    {
        return hitmanFOV.GetFirstVisibleObject(this.transform, _aspectNames);
    }
    #region Debug
    private void OnValidate()
    {
        hitmanFOV = new FieldOfView(fieldOfView, sightDistance);
    }
    private void OnDrawGizmos()
    {
        hitmanFOV.OnDrawGizmos(this.transform, showDebug);
    }
    #endregion
}
