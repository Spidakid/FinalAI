using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBTn : MonoBehaviour
{
    public void GoToTestScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void StopApp()
    {
        Application.Quit();
    }
}
