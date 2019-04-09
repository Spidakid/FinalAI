using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PoopTouch : MonoBehaviour
{
    public Material mat;
    private void OnTriggerEnter(Collider other)
    {
        SmellAspect aspect = other.GetComponent<SmellAspect>();
        //Check if object has a smell aspect
        if (aspect != null)
        {
            //Check if agent is not an herb worm
            if (aspect.aspect != Aspect.AspectName.Herbworm)
            {
                aspect.isPoopedOn = true;//turn on smell status of agent covered in poop
                other.GetComponent<Renderer>().sharedMaterial = mat;
                Destroy(this.gameObject);//Destroy itself
            }
        }
    }
}
