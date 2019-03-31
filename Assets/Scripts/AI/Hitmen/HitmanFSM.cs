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
    [SerializeField]
    private GameObject smokeCloud;
    [Tooltip("Distance to avoid obstacles")]
    public float avoidDistance = 5;
    [Tooltip("Amount of smoke grenades hitman has")]
    private int smokeGrenades = 1;

    [Header("Assassinate State")]
    private GameObject contractTarget;
    private Aspect.AspectName contractAspect;
    public bool showDebug = false;
    private FieldOfView hitmanFOV;
    [HideInInspector]
    public bool isBeingChased = false;
    // Start is called before the first frame update
    void Start()
    {
        NextContract();
        Debug.Log("Contract: "+contractTarget);
        if (this.tag != "Hitman")
        {
            this.tag = "Hitman";
        }
        if (hitmanFOV == null)
        {
            hitmanFOV = new FieldOfView(fieldOfView, sightDistance);
        }
        hitmanFOV.Start();
    }
    /// <summary>
    /// Creates a Smoke Cloud
    /// </summary>
    public void CreateSmokeBubble()
    {
        GameObject smokeobj;
        //Check if grenades are available
        if (smokeGrenades != 0)
        {
            smokeobj = Instantiate(smokeCloud, this.transform.position, Quaternion.Euler(Vector3.zero));
            smokeGrenades--;
        }
    }
    /// <summary>
    /// Get the target object if it is currently within field of view
    /// </summary>
    /// <returns>returns the current contract target within field of view</returns>
    public GameObject GetVisibleTargetObject()
    {
        return hitmanFOV.GetTargetObject(this.transform,contractTarget);
    }
    #region Contract
    /// <summary>
    /// Randomly select the next target to assassinate.
    /// </summary>
    public void NextContract()
    {
        //Times to cycle to find a new target if none is found in previous cycles
        int CycleTime = 0;
        GameObject[] targetObjs;
        int randomSelect;
        string targetType = "";
        contractTarget = null;
        
        while (contractTarget == null && CycleTime < 3)
        {
            randomSelect = Random.Range(0, 2);
            switch (randomSelect)
            {
                case 0:
                    targetType = "GovForce";
                    break;
                case 1:
                    targetType = "Boid";
                    break;
                //case 2:
                //    targetType = "Pacifist";
                //break;
                default:
                    CycleTime++;
                    break;
            }
            targetObjs = GameObject.FindGameObjectsWithTag(targetType);
            randomSelect = Random.Range(0,targetObjs.Length);
            //set new contract speculations
            contractTarget = targetObjs[randomSelect];
            contractAspect = contractTarget.GetComponent<SightTouchAspect>().aspect;
        }
        
        
    }
    public GameObject GetContractObject()
    {
        return contractTarget;
    }
    public Aspect.AspectName GetContractAspect()
    {
        return contractAspect;
    }
    #endregion
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
