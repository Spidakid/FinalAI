using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CameraSwitch : MonoBehaviour
{
    private List<Transform> cameraTrans;
    public Text camLocText;
    // Start is called before the first frame update
    void Start()
    {
        Camera.main.gameObject.SetActive(false);
        camLocText.text = "freecam";
        cameraTrans = new List<Transform>();
        RetrieveAllCameraTrans();
    }

    // Update is called once per frame
    void Update()
    {
        SwitchCameraInput();
    }
    private void RetrieveAllCameraTrans()
    {
        int totalChildCameras = this.gameObject.transform.childCount;
        for (int i = 0; i < totalChildCameras; i++)
        {
            cameraTrans.Add(this.gameObject.transform.GetChild(i));
        }
    }
    /// <summary>
    /// Switch to a camera around the outer bounds of the arena using keyboard inputs
    /// </summary>
    private void SwitchCameraInput()
    {
        //NorthWest
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ActivateCurCamera("northwest");
        }
        //North
        else if (Input.GetKeyDown(KeyCode.W))
        {
            ActivateCurCamera("north");
        }
        //NorthEast
        else if (Input.GetKeyDown(KeyCode.E))
        {
            ActivateCurCamera("northeast");
        }
        //East
        else if (Input.GetKeyDown(KeyCode.D))
        {
            ActivateCurCamera("east");
        }
        //SouthEast
        else if (Input.GetKeyDown(KeyCode.C))
        {
            ActivateCurCamera("southeast");
        }
        //South
        else if (Input.GetKeyDown(KeyCode.X))
        {
            ActivateCurCamera("south");
        }
        //SouthWest
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            ActivateCurCamera("southwest");
        }
        //West
        else if (Input.GetKeyDown(KeyCode.A))
        {
            ActivateCurCamera("west");
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            ActivateCurCamera("freecam");
        }
    }
    /// <summary>
    /// Activates a selected camera & disables every other camera
    /// </summary>
    /// <param name="_name"></param>
    private void ActivateCurCamera(string _name)
    {
        int totalChildCameras = this.gameObject.transform.childCount;
        for (int i = 0; i < totalChildCameras; i++)
        {
            if (this.gameObject.transform.GetChild(i).name.ToLower() == _name.ToLower())
            {
                this.gameObject.transform.GetChild(i).gameObject.SetActive(true);
                //set the name of camera location
                camLocText.text = _name;
            }
            else if(this.gameObject.transform.GetChild(i).gameObject.activeSelf)
            {
                this.gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    
}
