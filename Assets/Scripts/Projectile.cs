using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Speed = 0.2f;
    public bool showDebug = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(this.transform.forward * Speed * Time.deltaTime,Space.World);
    }
    private void OnTriggerEnter(Collider other)
    {
        SightTouchAspect aspectRef = other.GetComponent<SightTouchAspect>();
        if (aspectRef != null)
        {
            if (aspectRef.aspect == Aspect.AspectName.GovForce ||
           aspectRef.aspect == Aspect.AspectName.Hitman ||
           aspectRef.aspect == Aspect.AspectName.Islander ||
           aspectRef.aspect == Aspect.AspectName.Herbworm)
            {
                this.gameObject.SetActive(false);
                other.gameObject.SetActive(false);
                if (showDebug)
                {
                    Debug.Log(other.gameObject);
                }
            }
        }
        else if (other.tag.ToLower() == "obstacle" || other.tag.ToLower() == "wall")
        {
            this.gameObject.SetActive(false);
            if (showDebug)
            {
                Debug.Log(other.gameObject);
            }
        }
    }
}
