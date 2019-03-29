using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class BoidFeeling : MonoBehaviour
{
    [Tooltip("The radius to switch all boids within flock origin to AStar Mode. *Activates when obstacles are detected within this range*")]
    public float astarRadiusMode = 10f;
    Rigidbody rigid;
    SphereCollider collider;
    [HideInInspector]
    public bool obstacleDetected = false;
    // Start is called before the first frame update
    void Start()
    {
        #region sphere collider presets
        collider = this.GetComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = astarRadiusMode;//set collider radius to astar detection mode
        #endregion
        #region rigidbody presets
        rigid = this.GetComponent<Rigidbody>();
        rigid.useGravity = false;
        rigid.isKinematic = true;
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.ToLower() == "obstacle")
        {
            obstacleDetected = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.ToLower() == "obstacle")
        {
            obstacleDetected = false;
        }
    }
    #region Debug
    private void OnValidate()
    {
        collider = this.GetComponent<SphereCollider>();
        collider.radius = astarRadiusMode;//set collider radius to astar detection mode
    }
    #endregion
}
