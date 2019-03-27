using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GovForceFSM : MonoBehaviour
{
    public Aspect.AspectName[] EnemyAspects = {
        Aspect.AspectName.GovForce
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

    private GovForceFOV GovForceFOV;
    // Start is called before the first frame update
    void Start()
    {
        if (GovForceFOV == null)
        {
            GovForceFOV = new GovForceFOV(fieldOfView, sightDistance);
        }
        ObjectPooling.Instance.InitializePool(bullet);
        GovForceFOV.Start();
    }

    // Update is called once per frame
    void Update()
    {
    }
    public List<GameObject> GetVisibleObjects()
    {
        return GovForceFOV.GetAllVisibleObjects(this.transform);
    }
    public GameObject GetFirstVisibleObject(params Aspect.AspectName[] _aspectNames)
    {
        return GovForceFOV.GetFirstVisibleObject(this.transform,_aspectNames);
    }
    #region Debug
    private void OnValidate()
    {
        GovForceFOV = new GovForceFOV(fieldOfView, sightDistance);
    }
    private void OnDrawGizmos()
    {
        GovForceFOV.OnDrawGizmos(this.transform,showDebug);
    }
    #endregion
}
