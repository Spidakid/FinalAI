using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class WormTouch : MonoBehaviour
{
    Rigidbody rb;
    BoxCollider collider;
    Animator parentAnim;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        collider = this.GetComponent<BoxCollider>();
        collider.isTrigger = true;

        parentAnim = this.GetComponentInParent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        SmellAspect aspect = other.gameObject.GetComponent<SmellAspect>();
        //Check if object hasa smell aspect
        if (aspect != null)
        {
            //Check if Agent is covered in poop
            if (aspect.isPoopedOn)
            {
                other.gameObject.SetActive(false);
                //transition to Grow state
                //parentAnim.SetTrigger("Grow");
            }
        }
    }
}
