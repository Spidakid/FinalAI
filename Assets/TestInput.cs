using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestInput : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //Reset
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadSceneAsync("SampleScene");
        }
        //Close App
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadSceneAsync("Menu");
        }
        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.P))
        {
            if(Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }
        }
    }
}
