using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNavigation : MonoBehaviour
{
    public  Transform startPos, goalPos;
    private Node startNode, goalNode;

    public ArrayList pathArray;

    //GameObject objStartCube, objEndCube;

    private float currentTime = 0.0f;
    public float intervalTime = 1.0f; //Interval time between path finding
    public Color pathColor = Color.yellow;
    // Use this for initialization
    void Start()
    {
        //objStartCube = GameObject.FindGameObjectWithTag("Start");
        //objEndCube = GameObject.FindGameObjectWithTag("End");

        //AStar Calculated Path
        pathArray = new ArrayList();
        FindPath();
    }

    // Update is called once per frame
    void Update()
    {
        //Timer
        currentTime += Time.deltaTime;
        //Finds a path every interval
        if (currentTime >= intervalTime)
        {
            currentTime = 0.0f;
            FindPath();
        }
    }

    void FindPath()
    {
        //startPos = objStartCube.transform;
        //goalPos = objEndCube.transform;

        //Assign StartNode and Goal Node
        //startNode = new Node(GridManager.Instance.GetGridTileCenter(GridManager.Instance.GetGridIndex(startPos.position)));
        //goalNode = new Node(GridManager.Instance.GetGridTileCenter(GridManager.Instance.GetGridIndex(goalPos.position)));
        startNode = GetNodeForGrid(startPos.position);
        goalNode = GetNodeForGrid(goalPos.position);

        pathArray = AStar.FindPath(startNode, goalNode);
    }
    private Node GetNodeForGrid(Vector3 _pos)
    {
        return new Node(GridManager.Instance.GetGridTileCenter(GridManager.Instance.GetGridIndex(_pos))); ;
    }
    void OnDrawGizmos()
    {
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
                    Debug.DrawLine(node.position, nextNode.position, pathColor);
                    index++;
                }
            };
        }
    }
}
