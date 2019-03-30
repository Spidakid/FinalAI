using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasDisplay : MonoBehaviour
{
    private float currentTime;
    [SerializeField]
    private float displayTimer = 5;
    private bool startTimer = false;
    private Transform canvas;
    private Aspect.AspectName otherAspect;
    // Start is called before the first frame update
    void Start()
    {
        canvas = this.transform.Find("Canvas");
        canvas.gameObject.SetActive(false);//turn display off at start
    }

    // Update is called once per frame
    void Update()
    {
        if (startTimer)
        {
            if (!canvas.gameObject.activeSelf)
            {
                canvas.GetComponentInChildren<Text>().text = "Hello " + otherAspect + "!"; ;
                canvas.gameObject.SetActive(true);
            }
            currentTime += Time.deltaTime;
            if (currentTime >= displayTimer)
            {
                canvas.gameObject.SetActive(false);//turn display off
                currentTime = 0;
                startTimer = false;
            }
        }
    }
    public void ResetDisplayTime(Aspect.AspectName _otheraspect)
    {
        otherAspect = _otheraspect;
        currentTime = 0;
        startTimer = true;
    }
}
