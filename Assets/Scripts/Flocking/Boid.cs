using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public GameObject Leader;
    public GameObject flockOrigin;
    //current boid kinetic Speed
    private float boidSpeed = 0.4f;

    [Header("Separation Parameters")]
    [Tooltip("sphere radius to avoid other boids")]
    public float avoidanceRadius = 2.77f;
    private Color avoidanceColor = Color.green;

    //Cohesion
    private BoidOrigin flockOriginStats;
    [HideInInspector]
    public bool isWithinOrigin = false;//is boid in within the flocks origin

    //Alignment
    [Header("Alignment Parameters")]
    [Tooltip("time it takes to update the random speed and random direction")]
    public float maxAlignTime = 2f;
    private float curAlignTime = 0;//timer
    private float adjustSpeed;//How much faster to adjust to the flocking rules. Zero is same speed as flock Origin
    [Tooltip("Random speed within the range of the flocking origin")]
    public bool randomSpeed = false;

    private GameObject waypoint;
    private List<GameObject> otherBoids;
    private Vector3 movementDir = Vector3.zero;//Direction boid will move depending on boid rules
    private bool passBoidRules = true;//pass all flocking rules
    private float flockSpeed;
    [Space(10)]
    public bool showDebug = false;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.rotation = Quaternion.Euler(Vector3.zero);//reset rotation to zero
        otherBoids = new List<GameObject>();
        flockOriginStats = flockOrigin.GetComponent<BoidOrigin>();
        //Sets object tag as Boid
        if (this.tag.ToLower() == "untagged")
        {
            this.tag = "Boid";
        }
        if (Leader.tag.ToLower() == "untagged")
        {
            Leader.tag = "Boid";
        }
        //Retrieve all other boid gameobjects
        otherBoids.AddRange(GameObject.FindGameObjectsWithTag(this.tag));
        otherBoids.Remove(this.gameObject);

        flockSpeed = flockOrigin.GetComponent<AStarOrigin>().Speed;
        adjustSpeed = flockSpeed + boidSpeed;
        SetBoidSpeed();
        movementDir = Leader.transform.position - flockOrigin.transform.position;  
    }

    // Update is called once per frame
    void Update()
    {
        //Reset flocking checks
        passBoidRules = true;
        SeparationRule();
        CohesionRule();
        //Check if goal is reached and if flocking checks have been passed
        if (!flockOrigin.GetComponent<AStarOrigin>().reachedGoal && passBoidRules)
        {
            AlignmentRule();
        }
        else if (!passBoidRules)
        {
            boidSpeed = adjustSpeed;
        }
        if (!flockOrigin.GetComponent<AStarOrigin>().reachedGoal || !passBoidRules)
        {
            movementDir = new Vector3(movementDir.x,0,movementDir.z);
            //Normalize Vector
            movementDir.Normalize();
            //move boid
            this.transform.Translate(movementDir * boidSpeed * Time.deltaTime);
        }
    }
    #region Flocking Rules
    /// <summary>
    /// Maintains the distance with other neighbors in the flock to avoid collision
    /// </summary>
    private void SeparationRule()
    {
        //Vector3 DirFromBoid = Vector3.zero;
        for (int i = 0; i < otherBoids.Count; i++)
        {
            //Check if other boids is within the avoidance radius of this boid
            if (Vector3.Distance(this.transform.position, otherBoids[i].transform.position) < avoidanceRadius)
            {
                if (showDebug)
                {
                    Debug.Log(otherBoids[i].name);
                }
                passBoidRules = false;//Did not pass Rule
                //obtain the direction away from other boids
                movementDir += this.transform.position-otherBoids[i].transform.position;//[NOTE:]Direction from otherboid to this boid
            }
        }
    }
    /// <summary>
    /// Maintains a minimum distance with the flock's center
    /// </summary>
    private void CohesionRule()
    {
        //Check if boid is far away or too close from flock origin 
        if (Vector3.Distance(this.transform.position, flockOrigin.transform.position) >flockOriginStats.outerOriginRadius)
        {
            movementDir += flockOrigin.transform.position - this.transform.position;//[NOTE:]Direction from this boid to origin
            passBoidRules = false;
            isWithinOrigin = false;
        }
        else if (Vector3.Distance(this.transform.position, flockOrigin.transform.position) < BoidOrigin.innerOriginRadius)
        {
            movementDir += this.transform.position - flockOrigin.transform.position;//[NOTE:]Direction from origin to this boid
            passBoidRules = false;
            isWithinOrigin = false;
        }
        else
        {
            isWithinOrigin = true;
        }
    }
    /// <summary>
    /// Move in the same direction & velocity
    /// </summary>
    private void AlignmentRule()
    {
        //Align Speed
        curAlignTime += Time.deltaTime;
        if (curAlignTime >= maxAlignTime)
        {
            curAlignTime = 0;
            SetBoidSpeed();
            movementDir = Leader.transform.position - flockOrigin.transform.position;
        }

    }
    #endregion
    /// <summary>
    /// Set Boid speed based on if random option was selected
    /// </summary>
    private void SetBoidSpeed()
    {
        if (randomSpeed)
        {
            boidSpeed = Random.Range(flockSpeed - (flockSpeed * 0.9f), flockSpeed + Random.Range(0, 0.1f));
        }
        else
        {
            boidSpeed = flockSpeed * 0.8f;
        }
    }
    #region Debug
    private void OnDrawGizmos()
    {
        if (showDebug)
        {
            DrawAvoidanceParams();
        }
    }
    /// <summary>
    /// Draws the avoidance properties in the Scene view
    /// </summary>
    private void DrawAvoidanceParams()
    {
        Gizmos.color = avoidanceColor;
        Gizmos.DrawWireSphere(this.transform.position, avoidanceRadius);
    }
    #endregion
}
