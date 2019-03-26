using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GovForceFSM : MonoBehaviour
{
    [Header("Field Of Vision")]
    public float sightDistance;
    [Range(0,180)]
    public float fieldOfView = 45f;

    [Header("Shoot State")]
    public GameObject bullet;
    [Tooltip("Shoot every X seconds")]
    public float shootInterval = 3f;
    [Tooltip("Stopping range in which to shoot while chasing")]
    public float shootRange=2f;
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
        GovForceFOV.Update(this.transform);
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
