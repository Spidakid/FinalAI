using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class BoidHearing : MonoBehaviour
{
    public float hearRadius = 5;
    public bool showDebug = false;
    private Rigidbody rb;
    private SphereCollider collider;
    private float currentTime;
    [SerializeField,Tooltip("The amount of time to display dialogue textbox")]
    private float dialogueTime = 5;
    private bool startTimer = false;
    // Start is called before the first frame update
    void Start()
    {
        collider = this.GetComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = hearRadius;
        rb = this.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        //init timer
        currentTime = 0;
    }
    private void Update()
    {
        if (startTimer)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= dialogueTime)
            {
                this.transform.Find("Canvas").gameObject.SetActive(false);
                startTimer = false;
            }
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        HearAspect hearobj = other.GetComponent<HearAspect>();
        //Check if object has a hearing aspect
        if (hearobj != null)
        {
            //Check if hearing aspect is an islander
            if (hearobj.aspect != Aspect.AspectName.Islander)
            {
                
                ResetThisCanvasTimer(hearobj);
                if (hearobj.aspect == Aspect.AspectName.GovForce)
                {
                    ResetAllyCanvasTimer(hearobj);
                }
                if (showDebug)
                {
                    Debug.Log("Sound Detected!");
                }
                
            }
        }
    }
    /// <summary>
    /// Reset the amount of time the canvas is displayed
    /// </summary>
    /// <param name="_hearobj"></param>
    private void ResetThisCanvasTimer(HearAspect _hearobj)
    {
        startTimer = true;//start the timer calculations
        //Reset Timer to 0
        currentTime = 0;
        //Canvas Ref of this object
        Transform canvas = this.transform.Find("Canvas");
        //Display new message onto Canvas
        canvas.Find("Text").GetComponent<Text>().text = "Hello " + _hearobj.aspect + "!";
        canvas.gameObject.SetActive(true);
    }
    /// <summary>
    /// Reset the amount of time the canvas is displayed for the allies of the islanders;
    /// </summary>
    private void ResetAllyCanvasTimer(HearAspect _hearobj)
    {
        CanvasDisplay otherCanvas = _hearobj.GetComponentInParent<CanvasDisplay>();
        otherCanvas.ResetDisplayTime(this.GetComponentInParent<SightTouchAspect>().aspect);
    }
    #region Debug
    private void OnValidate()
    {
        collider = this.GetComponent<SphereCollider>();
        collider.radius = hearRadius;
    }
    #endregion
}
