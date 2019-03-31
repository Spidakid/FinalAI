using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class HitmanTouch : MonoBehaviour
{
    HitmanFSM hitFSM;
    BoxCollider collider;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        hitFSM = this.GetComponentInParent<HitmanFSM>();
        rb = this.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        collider = this.GetComponent<BoxCollider>();
        collider.isTrigger = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == hitFSM.GetContractObject())
        {
            //disable contract target
            other.gameObject.SetActive(false);
        }
    }
}
