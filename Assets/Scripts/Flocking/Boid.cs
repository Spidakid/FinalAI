using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public GameObject Leader;
    public float boidRadius = 1f;
    public float maxDistanceFromLeader;
    private GameObject waypoint;
    GameObject[] otherBoids;
    private Vector3 DirFromBoid;
    private float distanceFromLeader;
    
    // Start is called before the first frame update
    void Start()
    {
        if (this.tag.ToLower() == "untagged")
        {
            this.tag = "Boid";

        }
        //create a waypoint
        waypoint = new GameObject(this.name +"'s waypoint");
        otherBoids = GameObject.FindGameObjectsWithTag(this.tag);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void SeparationRule()
    {
        for (int i = 0; i < otherBoids.Length; i++)
        {

        }
    }
    private void CohesionRule()
    {

    }
    private void AlignmentRule()
    {

    }
}
