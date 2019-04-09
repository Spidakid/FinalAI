using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GovForceFSM : MonoBehaviour
{
    public Aspect.AspectName[] EnemyAspects = {
        Aspect.AspectName.Hitman,
        Aspect.AspectName.Herbworm
    };
    [Header("Field Of Vision")]
    public float sightDistance;
    [Range(0,180)]
    public float fieldOfView = 45f;

    [Header("Shoot State")]
    public GameObject bullet;
    [Tooltip("Shoot every X seconds")]
    public float shootInterval = 3f;
    [Tooltip("Stopping range in which to shoot while chasing")]
    public float startShootingRange = 2.5f;
    [Tooltip("How fast to turn while in shoot state")]
    public float turnSpeed = 2f;
    public bool showDebug = false;
    [HideInInspector]
    public bool isVisionOn = true;//can GovForce see?
    private FieldOfView GovForceFOV;
    // Start is called before the first frame update
    void Start()
    {
        if (this.tag != "GovForce")
        {
            this.tag = "GovForce";
        }
        if (GovForceFOV == null)
        {
            GovForceFOV = new FieldOfView(fieldOfView, sightDistance);
        }
        ObjectPooling.Instance.InitializePool(bullet);
        GovForceFOV.Start();
    }
    /// <summary>
    /// Get All visible objects not in the ignore raycast layer
    /// </summary>
    /// <returns>returns a list of all objects visible</returns>
    public List<GameObject> GetVisibleObjects()
    {
        if (isVisionOn)
        {
            return GovForceFOV.GetAllVisibleObjects(this.transform);
        }
        return null;
    }
    /// <summary>
    /// Retrieve the first visible object based on the specified apsects to look for
    /// </summary>
    /// <param name="_aspectNames">Aspect(s) to look out for</param>
    /// <returns>Retrieve the first visible object of specficied aspect(s)</returns>
    public GameObject GetFirstVisibleObject(params Aspect.AspectName[] _aspectNames)
    {
        if (isVisionOn)
        {
            return GovForceFOV.GetFirstVisibleObject(this.transform, _aspectNames);
        }
        return null;
    }
    #region Debug
    private void OnValidate()
    {
        GovForceFOV = new FieldOfView(fieldOfView, sightDistance);
    }
    private void OnDrawGizmos()
    {
        if (isVisionOn)
        {
            GovForceFOV.OnDrawGizmos(this.transform, showDebug);
        }
        
    }
    #endregion
}
