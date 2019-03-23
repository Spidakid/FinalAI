using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public GameObject Leader;
    public GameObject flockOrigin;

    [Header("Separation Parameters")]
    public float separationSpeed = 0.15f;
    [Tooltip("sphere radius to avoid other boids")]
    public float avoidanceRadius = 5f;
    private Color avoidanceColor = Color.green;

    //Avoid Origin Parameters
    [Header("Cohesion Parameters")]
    [Tooltip("How fast to place boid back within the radius of the flock's origin")]
    public float avoidOriginSpeed = 0.2f;
    [Tooltip("Maximum distance to be away origin")]
    public float outerOriginRadius = 5f;
    [Tooltip("Minimum from the center of the origin")]
    private static float innerOriginRadius = 0.8f;
    public Color originOuterColor = Color.black;
    private static Color originInnerColor = Color.black;

    private GameObject waypoint;
    private List<GameObject> otherBoids;

    // Start is called before the first frame update
    void Start()
    {
        otherBoids = new List<GameObject>();
        //Sets object tag as Boid
        if (this.tag.ToLower() == "untagged")
        {
            this.tag = "Boid";
        }
        if (Leader.tag.ToLower() == "untagged")
        {
            Leader.tag = "Boid";
        }
        //create a waypoint
        waypoint = new GameObject(this.name +"'s waypoint");
        //Retrieve all other boid gameobjects
        otherBoids.AddRange(GameObject.FindGameObjectsWithTag(this.tag));
        otherBoids.Remove(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        SeparationRule();
        CohesionRule();
    }
    /// <summary>
    /// Maintains the distance with other neighbors in the flock to avoid collision
    /// </summary>
    private void SeparationRule()
    {
        Vector3 DirFromBoid = Vector3.zero;
        for (int i = 0; i < otherBoids.Count; i++)
        {
            //Check if other boids is within the avoidance radius of this boid
            if (Vector3.Distance(this.transform.position, otherBoids[i].transform.position) < avoidanceRadius)
            {
                Debug.Log(otherBoids[i].name);
                //obtain the direction away from other boids
                DirFromBoid += this.transform.position-otherBoids[i].transform.position;//[NOTE:]Direction from otherboid to this boid
            }
        }
        DirFromBoid.Normalize();
        //moves this boid away from neighboring boids
        this.transform.Translate(DirFromBoid * separationSpeed * Time.deltaTime);
    }
    /// <summary>
    /// Maintains a minimum distance with the flock's center
    /// </summary>
    private void CohesionRule()
    {
        
        if (Vector3.Distance(this.transform.position, flockOrigin.transform.position) > outerOriginRadius)
        {
            Vector3 DirToOrigin = flockOrigin.transform.position - this.transform.position;//[NOTE:]Direction from this boid to origin
            //moves this boid towards the flock's center
            this.transform.Translate(DirToOrigin * avoidOriginSpeed * Time.deltaTime);
            
        }
        else if (Vector3.Distance(this.transform.position, flockOrigin.transform.position) < innerOriginRadius)
        {
            Vector3 DirFromOrigin = this.transform.position - flockOrigin.transform.position;//[NOTE:]Direction from origin to this boid
            //moves this boid away from the flock's center
            this.transform.Translate(DirFromOrigin * avoidOriginSpeed *Time.deltaTime);
        }
    }
    private void AlignmentRule()
    {

    }
    #region Debug
    private void OnDrawGizmos()
    {
        DrawAvoidanceParams();
        DrawCohesionParams();
    }
    /// <summary>
    /// Draws the avoidance properties in the Scene view
    /// </summary>
    private void DrawAvoidanceParams()
    {
        Gizmos.color = avoidanceColor;
        Gizmos.DrawWireSphere(this.transform.position, avoidanceRadius);
    }
    /// <summary>
    /// Draws the cohesion properties in the Scene view
    /// </summary>
    private void DrawCohesionParams()
    {
        //minimum flock origin radius
        Gizmos.color = originInnerColor;
        Gizmos.DrawSphere(flockOrigin.transform.position,innerOriginRadius);
        //maximum flock origin radius
        Gizmos.color = originOuterColor;
        Gizmos.DrawWireSphere(flockOrigin.transform.position,outerOriginRadius);
    }
    #endregion
}
