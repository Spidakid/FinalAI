using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNavigation : MonoBehaviour
{
    public  Transform startPos, goalPos;
    private Node startNode, goalNode;

    public ArrayList pathArray;

    private float currentTime = 0.0f;
    public float intervalTime = 1.0f; //Interval time between path finding
    public float Speed = 1.0f;
    public Color lineColor = Color.yellow;
    public float stopRadius = 0.5f;
    public Color sphereColor = Color.magenta;
    private int curPathindex;
    // Use this for initialization
    void Start()
    {
        //AStar Calculated Path
        pathArray = new ArrayList();
        FindPath();
        curPathindex = 0;//init
    }

    // Update is called once per frame
    void Update()
    {
        //Check if current index is within the bounds of the pathArray
        if (curPathindex < pathArray.Count)
        {
            //Timer
            currentTime += Time.deltaTime;
            //Finds a path every interval
            if (currentTime >= intervalTime)
            {
                Debug.Log("RESET!");
                ResetPathfinding();
            }
            FollowPath();
        }
    }
    /// <summary>
    /// Finds a new path
    /// </summary>
    void FindPath()
    {
        startNode = GetNodeForGrid(startPos.position);
        goalNode = GetNodeForGrid(goalPos.position);

        pathArray = AStar.FindPath(startNode, goalNode);
    }
    private Node GetNodeForGrid(Vector3 _pos)
    {
        return new Node(GridManager.Instance.GetGridTileCenter(GridManager.Instance.GetGridIndex(_pos)));
    }
    /// <summary>
    /// Follow and Steer towards the current generated path
    /// </summary>
    void FollowPath()
    {
        //ternary operator assignment
        //  if curPathIndex < pathArray.Count then curNode = (Node)pathArray[curPathindex];
        //  else curNode = null;
        Node curNode = (curPathindex < pathArray.Count) ? (Node)pathArray[curPathindex] : null;
        //Check if array or current node is null
        if (pathArray == null ||curNode == null)
        {
            return;
        }
        else if (Vector3.Distance(this.transform.position, curNode.position) < this.stopRadius)
        {
            curPathindex++;
            //Reset index to 0 if out of bounds
            if (curPathindex >= pathArray.Count)
            {
                curPathindex = 0;
            }
        }
        Vector3 objToCurNode = curNode.position - this.transform.position;
        this.transform.rotation = Quaternion.LookRotation(objToCurNode);//Rotate Agent to face node
        this.transform.Translate(Vector3.forward * Speed * Time.deltaTime);//Move Agent
        

    }
    /// <summary>
    /// Restart the Pathfinding process
    /// </summary>
    void ResetPathfinding()
    {
        //reset refresh timer
        currentTime = 0.0f;
        
        //Generate new A* path
        FindPath();
    }
    #region Debug
    void OnDrawGizmos()
    {
        Debug.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 3, Color.red);
        if (pathArray == null)
            return;

        if (pathArray.Count > 0)
        {
            int index = 1;
            foreach (Node node in pathArray)
            {
                if (index < pathArray.Count)
                {
                    Node nextNode = (Node)pathArray[index];
                    Debug.DrawLine(node.position, nextNode.position, lineColor);
                    //Sphere Debug Drawing
                    Gizmos.color = sphereColor;
                    Gizmos.DrawSphere(node.position,stopRadius);
                    index++;
                }
            };
        }
    }
    #endregion
}
