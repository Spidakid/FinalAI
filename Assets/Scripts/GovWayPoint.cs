using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GovWayPoint : MonoBehaviour
{
    public Rect boundaryRect;
    private BoxCollider boxCollider;
    private Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = this.GetComponent<BoxCollider>();
        boxCollider.size = new Vector3(0.2f, 0.2f, 0.2f);
        boxCollider.isTrigger = true;

        rigid = this.GetComponent<Rigidbody>();
        rigid.useGravity = false;
        rigid.isKinematic = true;
    }

    public Vector3 GetRandomDestination()
    {
        return new Vector3(Random.Range(boundaryRect.x, boundaryRect.width), this.transform.position.y, Random.Range(boundaryRect.y, boundaryRect.height));
    }
    private void OnTriggerEnter(Collider other)
    {
        //Check if waypoint is placed on a wall or obstacle
        if (other.tag.ToLower() == "obstacle" || other.tag.ToLower() == "wall")
        {
            this.transform.position = GetRandomDestination();
        }
        else if (other.GetComponent<SightTouchAspect>() != null && other.GetComponent<SightTouchAspect>().aspect == Aspect.AspectName.Hitman)
        {
            //Check if waypoint is set to Hitman's position
            if (this.transform.position == other.gameObject.transform.position)
            {
                other.gameObject.GetComponent<HitmanFSM>().isBeingChased = true;
                Debug.Log(other);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<SightTouchAspect>() != null && other.GetComponent<SightTouchAspect>().aspect == Aspect.AspectName.Hitman)
        {
            other.gameObject.GetComponent<HitmanFSM>().isBeingChased = false;
        }
    }
}
