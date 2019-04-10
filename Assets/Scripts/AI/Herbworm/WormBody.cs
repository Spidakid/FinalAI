using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormBody : MonoBehaviour
{
    [HideInInspector]
    public GameObject head;
    // Update is called once per frame
    void Update()
    {
        if (head != null)
        {
            //Check if the head is active
            if (!head.activeSelf)
            {
                //Deactivate body if head doesn't exist
                this.gameObject.SetActive(false);
            }
        }
        
    }
}
