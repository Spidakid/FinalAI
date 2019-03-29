using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarOrigin : MonoBehaviour
{
    private BoidOrigin boidOrigin;
    private Transform startPos;
    [Tooltip("The object to reach")]
    public Transform goalPos;
    private Node startNode, goalNode;
    [HideInInspector]
    public float Speed;

    public ArrayList pathArray;
    //Time
    private float currentTime = 0.0f;
    [HideInInspector]
    public float intervalTime = 1.0f; //Interval time between path finding

    public Color lineColor = Color.yellow;
    public float nodeRadius = 0.5f;
    public Color nodeColor = Color.magenta;

    private int curPathindex;
    [HideInInspector]
    public bool isStopped = false;//stops the path follow, but keeps pathfinding
    [HideInInspector]
    public bool reachedGoal = false;//reached goal
    public bool showDebug = false;
    // Use this for initialization
    void Start()
    {
        //AStar Calculated Path
        pathArray = new ArrayList();
        startPos = this.transform;
        FindPath();
        //Set previous path count
        //prevPathCount = pathArray.Count;
        curPathindex = 0;//init
        this.intervalTime = goalPos.GetComponent<AStarNavigation>().intervalTime;
        this.Speed = goalPos.GetComponent<AStarNavigation>().Speed;
        //flock origin ref
        boidOrigin = this.GetComponent<BoidOrigin>();
    }

    // Update is called once per frame
    void Update()
    {
        ////Timer
        RefreshPathTimer();
        if (!isStopped)
        {
            //Check if current index is within the bounds of the pathArray
            if (curPathindex < pathArray.Count)
            {
                FollowPath();
            }
        }

    }
    /// <summary>
    /// Finds a path every interval
    /// </summary>
    private void RefreshPathTimer()
    {
        currentTime += Time.deltaTime;
        ////Finds a path every interval
        if (currentTime >= intervalTime)
        {
            if (showDebug)
            {
                Debug.Log("RESET!");
            }
            
            ResetPathfinding();
        }
    }
    /// <summary>
    /// Finds a new path
    /// </summary>
    void FindPath()
    {
        startNode = GetNodeForGrid(startPos.position);
        goalNode = GetNodeForGrid(goalPos.position);


        if (pathArray.Count > 0)
        {
            pathArray.Clear();
        }
        //Convert PathArray list to hold only type Vector3 rather than Node
        ArrayList nodeArray = AStar.FindPath(startNode, goalNode);//holds nodes
        Vector3[] posArray = new Vector3[nodeArray.Count + 1];//holds vector3s
        for (int i = 0; i < posArray.Length; i++)
        {
            if (i == 0)
            {
                posArray[i] = goalPos.position;
            }
            else
            {
                Node curnode = (Node)nodeArray[i - 1];
                posArray[i] = curnode.position;
            }
        }
        pathArray.AddRange(posArray);//add contents of array into Arraylist
        pathArray.Reverse();//***Since we want a path array from the start node to the target node, we call this method;

    }
    /// <summary>
    /// Generate a new Node to be placed on the grid
    /// </summary>
    /// <param name="_pos"></param>
    /// <returns></returns>
    private Node GetNodeForGrid(Vector3 _pos)
    {
        return new Node(GridManager.Instance.GetGridTileCenter(GridManager.Instance.GetGridIndex(_pos)));
    }
    /// <summary>
    /// Follow and Steer towards the current generated path
    /// </summary>
    private void FollowPath()
    {
        //ternary operator assignment
        //  if curPathIndex < pathArray.Count then curNode = (Vector3)pathArray[curPathindex];
        //  else curNode = Vector3.zero;
        Vector3 curNodeVector = (curPathindex < pathArray.Count) ? (Vector3)pathArray[curPathindex] : Vector3.zero;
        //Check if array or current node is null
        if (pathArray == null || curNodeVector == Vector3.zero)
        {
            return;
        }
        else if (curNodeVector == goalPos.position && Vector3.Distance(this.transform.position, goalPos.position) < boidOrigin.distanceFromLeader || Vector3.Distance(this.transform.position, goalPos.position) < boidOrigin.distanceFromLeader)
        {
            if (!reachedGoal)
            {
                reachedGoal = true;
            }
            if (showDebug)
            {
                Debug.Log("Goal Reached!");
            }
            return;
        }
        else if (Vector3.Distance(this.transform.position, curNodeVector) < this.nodeRadius)
        {
            
            //Check if current node reached the goal
            if (curNodeVector == goalPos.position)
            {
                if (!reachedGoal)
                {
                    reachedGoal = true;
                }
                if (showDebug)
                {
                    Debug.Log("Goal Reached!");
                }  
                return;
            }
            curPathindex++;
            ResetPathfinding();
            //Check if current path index is out of bounds
            if (curPathindex >= pathArray.Count)
            {
                curPathindex = 0;
            }
        }
        if (reachedGoal)
        {
            reachedGoal = false;
        }
        //Path Follow and Steering algorithm
        Vector3 objToCurNode = curNodeVector - this.transform.position;
        this.transform.rotation = Quaternion.LookRotation(objToCurNode);//Rotate Agent to face node
        this.transform.Translate(Vector3.forward * Speed * Time.deltaTime);//Move Agent

    }
    /// <summary>
    /// Set a new destination to travel
    /// </summary>
    /// <param name="_pos"></param>
    public void ChangeGoalPosition(Vector3 _pos)
    {
        //set new goal position
        goalPos.position = _pos;
        //reset current path index
        curPathindex = 0;
        ResetPathfinding();
    }
    /// <summary>
    /// Restart the Pathfinding process
    /// </summary>
    private void ResetPathfinding()
    {
        //reset refresh timer
        currentTime = 0.0f;
        //Generate new A* path
        FindPath();
    }
    #region Debug
    void OnDrawGizmos()
    {
        if (showDebug)
        {
            if (pathArray == null)
                return;

            if (pathArray.Count > 0)
            {
                int index = 1;
                foreach (Vector3 position in pathArray)
                {
                    if (index < pathArray.Count)
                    {
                        Vector3 nextPosition = (Vector3)pathArray[index];
                        Debug.DrawLine(position, nextPosition, lineColor);
                        //Sphere Debug Drawing
                        Gizmos.color = nodeColor;
                        Gizmos.DrawSphere(position, nodeRadius);
                        index++;
                    }
                };
            }
        }
        
        
    }
    #endregion
}
