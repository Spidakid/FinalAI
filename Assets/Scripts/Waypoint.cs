using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider)),RequireComponent(typeof(Rigidbody))]
public class Waypoint : MonoBehaviour
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
         return new Vector3(Random.Range(boundaryRect.x,boundaryRect.width), this.transform.position.y, Random.Range(boundaryRect.y, boundaryRect.height));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.ToLower() == "obstacle" || other.tag.ToLower() == "wall")
        {
            this.transform.position = GetRandomDestination();
        }
    }
}
