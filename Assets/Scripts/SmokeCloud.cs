using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class SmokeCloud : MonoBehaviour
{
    [Tooltip("How fast to spread the smoke cloud. *Smaller value will reach max faster"),Range(0,10)]
    public float spreadSpeed = 2;
    [Tooltip("Max radius the smoke cloud will expand")]
    public float maxSpread = 4;
    Rigidbody rb;
    SphereCollider collider;
    Vector3 maxSpreadScale,curScale;
    List<SightTouchAspect> ObjectsInSmoke;
    // Start is called before the first frame update
    void Start()
    {
        ObjectsInSmoke = new List<SightTouchAspect>();
        rb = this.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        collider = this.GetComponent<SphereCollider>();
        collider.isTrigger = true;
        this.transform.localScale = Vector3.zero;
        maxSpreadScale = new Vector3(maxSpread,maxSpread, maxSpread);

    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.localScale.x >= maxSpreadScale.x-0.4f)
        {
            this.transform.localScale = Vector3.zero;
            this.gameObject.SetActive(false);
        }
        else
        {
            this.transform.localScale = Vector3.SmoothDamp(this.transform.localScale, maxSpreadScale, ref curScale, spreadSpeed);
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        SightTouchAspect aspectobj = other.GetComponent<SightTouchAspect>();
        if (aspectobj != null)
        {
            if (aspectobj.aspect == Aspect.AspectName.GovForce)
            {
                //Check if object is in Smoke
                if (ObjectsInSmoke.Count > 0)
                {
                    for (int i = 0; i < ObjectsInSmoke.Count; i++)
                    {
                        if (ObjectsInSmoke[i].gameObject != aspectobj.gameObject)
                        {
                            aspectobj.gameObject.GetComponent<GovForceFSM>().isVisionOn = false;
                            ObjectsInSmoke.Add(aspectobj);
                        }
                    }
                }
                else
                {
                    aspectobj.gameObject.GetComponent<GovForceFSM>().isVisionOn = false;
                    ObjectsInSmoke.Add(aspectobj);
                }
                
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        SightTouchAspect aspectobj = other.GetComponent<SightTouchAspect>();
        if (aspectobj != null)
        {
            if (aspectobj.aspect == Aspect.AspectName.GovForce)
            {
                //Check if Object is out of smoke
                for (int i = 0; i < ObjectsInSmoke.Count; i++)
                {
                    if (ObjectsInSmoke[i].gameObject == aspectobj.gameObject)
                    {
                        aspectobj.gameObject.GetComponent<GovForceFSM>().isVisionOn = true;
                        ObjectsInSmoke.Remove(aspectobj);
                    }
                }
            }
        }
    }
    private void OnDisable()
    {
        if (ObjectsInSmoke != null)
        {
            if (ObjectsInSmoke.Count > 0)
            {

                for (int i = 0; i < ObjectsInSmoke.Count; i++)
                {
                    ObjectsInSmoke[i].gameObject.GetComponent<GovForceFSM>().isVisionOn = true;
                }
                ObjectsInSmoke.Clear();
            }
        }
    }
}
