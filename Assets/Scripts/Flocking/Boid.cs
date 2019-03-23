using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public GameObject Leader;
    [Tooltip("sphere radius to avoid other boids")]
    public float boidRadius = 5f;
    public Color boidRadiusColor = Color.green;
    public float separationSpeed = 0.25f;
    private GameObject waypoint;
    private List<GameObject> otherBoids;
    private AStarNavigation astar;
    
    // Start is called before the first frame update
    void Start()
    {
        otherBoids = new List<GameObject>();
        //astar = this.GetComponent<AStarNavigation>();
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
    }
    private void SeparationRule()
    {
        Vector3 DirFromBoid = Vector3.zero; 
        for (int i = 0; i < otherBoids.Count; i++)
        {
            
            if (Vector3.Distance(this.transform.position, otherBoids[i].transform.position) < boidRadius)
            {
                Debug.Log(otherBoids[i].name);
                DirFromBoid += this.transform.position-otherBoids[i].transform.position;
            }
        }
        DirFromBoid.Normalize();
        this.transform.Translate(DirFromBoid * separationSpeed * Time.deltaTime);
    }
    private void CohesionRule()
    {

    }
    private void AlignmentRule()
    {

    }
    #region Debug
    private void OnDrawGizmos()
    {
        Gizmos.color = boidRadiusColor;
        Gizmos.DrawWireSphere(this.transform.position, boidRadius);
    }
    #endregion
}
