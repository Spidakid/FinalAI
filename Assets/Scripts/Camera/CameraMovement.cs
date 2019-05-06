using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float Speed = 10;
    private Vector3 Direction;
    // Start is called before the first frame update
    void Start()
    {
        Direction = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        MoveInput();
        this.transform.Translate(Direction * this.Speed * Time.deltaTime,Space.World);
        VerticalInput();
    }
    /// <summary>
    /// Move the camera on x-axis & z-axis;
    /// </summary>
    private void MoveInput()
    {
        Direction = Vector3.zero;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Direction = Vector3.forward;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Direction = Vector3.right;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
             Direction = Vector3.back;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Direction = Vector3.left;
        }
    }
    /// <summary>
    /// Move the camera up and down within a boundary
    /// </summary>
    private void VerticalInput()
    {
        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            if (this.transform.position.y > 10)
            {
                this.transform.position -= new Vector3(0,5,0);
            }
        }
        if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if (this.transform.position.y < 50)
            {
                this.transform.position += new Vector3(0, 5, 0);
            }
        }
    }
}
