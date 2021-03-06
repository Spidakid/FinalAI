﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormBody : MonoBehaviour
{
    [HideInInspector]
    public GameObject parentBody;
    // Update is called once per frame
    void Update()
    {
        if (parentBody != null)
        {
            //Check if the head is active
            if (!parentBody.activeSelf)
            {
                //Deactivate body if head doesn't exist
                this.gameObject.SetActive(false);
            }
        }
        
    }
}
