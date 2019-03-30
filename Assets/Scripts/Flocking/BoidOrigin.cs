using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidOrigin : MonoBehaviour
{
    //Cohesion
    [Header("Cohesion Parameters")]
    [Tooltip("How much faster to adjust the boid to the cohesion rules in astar mode. Zero is same speed as flock Origin")]
    public float astarSpeedAdjust = 0.4f;
    [Tooltip("Maximum distance to be away origin")]
    public float outerOriginRadius = 5f;
    [Tooltip("Minimum from the center of the origin")]
    public static float innerOriginRadius = 0.8f;
    public Color originOuterColor = Color.black;
    public static Color originInnerColor = Color.black;

    //Relationship with Leader
    [Header("Leader Relationship"),Space(10)]
    public GameObject leader;
    [Tooltip("Flock origin's distance from the Leader")]
    public float distanceFromLeader = 3.5f;

    public bool showDebug = true;
    // Start is called before the first frame update
    void Start()
    {
        //Set the flock origin a certain distance away from current leader
        this.transform.position = leader.transform.position + (leader.transform.forward * -1) * distanceFromLeader;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #region Debug
    private void OnValidate()
    {
        if (showDebug)
        {
            //this.transform.position = leader.transform.position + (leader.transform.forward * -1) * distanceFromLeader;
        }
    }
    private void OnDrawGizmos()
    {
        if (showDebug)
        {
            DrawCohesionParams();
        }
    }
    /// <summary>
    /// Draws the cohesion properties in the Scene view
    /// </summary>
    private void DrawCohesionParams()
    {
        //minimum flock origin radius
        Gizmos.color = BoidOrigin.originInnerColor;
        Gizmos.DrawSphere(this.transform.position, innerOriginRadius);
        //maximum flock origin radius
        Gizmos.color = originOuterColor;
        Gizmos.DrawWireSphere(this.transform.position, outerOriginRadius);
    }
    #endregion
}
