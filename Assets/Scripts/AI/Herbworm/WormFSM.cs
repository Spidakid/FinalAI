using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormFSM : MonoBehaviour
{
    public GameObject wormBody;
    [Header("Pooping")]
    public GameObject poopObject;
    [SerializeField,Tooltip("min time for the worm to poop")]
    private float minPoopTime = 20f;
    [SerializeField,Tooltip("max time for the worm to poop")]
    private float maxPoopTime = 45f;
    [HideInInspector]
    public float poopInterval;

    [Header("Smelling")]
    [Tooltip("The maximum smelling distance")]
    public float smellDistance = 5;
    [Tooltip("Width of sphere cast")]
    public float smellWidthRadius = 2;
    public float smellInterval = 1.4f;

    [Header("Prey")]
    public float chaseSpeed = 2f;
    [HideInInspector]
    public float initialSpeed;

    private Animator anim;
    [HideInInspector]
    public GameObject foodAgent;
    private GameObject lastBody;
    private bool isStartBody = true;
    // Start is called before the first frame update
    void Start()
    {
        initialSpeed = this.GetComponent<AStarNavigation>().Speed;
        anim = this.GetComponent<Animator>();
        lastBody = this.gameObject;
        poopInterval = Random.Range(minPoopTime,maxPoopTime);
    }
    /// <summary>
    /// Spawns poop at the last body of the worm
    /// </summary>
    public void SpawnPoop()
    {
        GameObject poop = Instantiate(poopObject);
        poop.transform.position = lastBody.transform.position;
        poopInterval = Random.Range(minPoopTime,maxPoopTime);
    }
    /// <summary>
    /// Smells a given area and detect if a nearby agent has poo on them
    /// </summary>
    public void PooSmellDetection()
    {
        RaycastHit hit;
        if (Physics.SphereCast(this.transform.position,smellWidthRadius,this.transform.forward,out hit,smellDistance))
        {
            SmellAspect aspect = hit.transform.gameObject.GetComponent<SmellAspect>();
            if (aspect != null)
            {
                if (aspect.isPoopedOn)
                {
                    //Set the position of the 
                    foodAgent = hit.transform.gameObject;
                    //transition to Prey State
                    anim.SetBool("SmellsPoo",true);
                    
                }
            }
        }
    }
    /// <summary>
    /// Attaches a new body on to the head of the worm 
    /// </summary>
    public void GrowNewBody()
    {
        int Xpos = Random.Range(0, 2);
        GameObject curBody = Instantiate(wormBody,this.transform.parent);
        switch (Xpos)
        {
            case 0:
                //set body to the left of last body
                curBody.transform.position = new Vector3(lastBody.transform.position.x + 2f, lastBody.transform.position.y, lastBody.transform.position.z);
                break;
            case 1:
                //set body to the left of last body
                curBody.transform.position = new Vector3(lastBody.transform.position.x + -2f, lastBody.transform.position.y, lastBody.transform.position.z);
                break;
        }
        curBody.GetComponent<AStarNavigation>().goalPos = lastBody.transform;//set body to follow last body attached to worm
        curBody.GetComponent<WormBody>().head = this.gameObject;//set the leader for the new body 
        lastBody = curBody;//assign current body as the last body attached to worm
        
    }
    private void OnDrawGizmos()
    {
        Debug.DrawLine(this.transform.position,this.transform.position + this.transform.forward * smellDistance,Color.red);
        Gizmos.DrawWireSphere(this.transform.position+this.transform.forward,smellWidthRadius);
    }
}
