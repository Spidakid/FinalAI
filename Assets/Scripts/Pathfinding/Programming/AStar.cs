using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    private static SortedList openList;
    private static HashSet<Node> closedList;

    /// <summary>
    /// Calculate the final path in the path finding
    /// </summary>
    private static ArrayList CalculatePath(Node _node)
    {
        ArrayList list = new ArrayList();
        //Checks the parents of all nodes until it reaches a node without a parent(the start node)
        while (_node != null)
        {
            //add node to the list
            list.Add(_node);
            //sets the current node's parent as the current node
            _node = _node.parent;
        }
        return list;
    }

    /// <summary>
    /// Calculate Cost
    /// </summary>
    private static float CostCalculation(Node _curNode, Node _goalNode)
    {
        Vector3 vecCost = _curNode.position - _goalNode.position;
        return vecCost.magnitude;
    }

    /// <summary>
    /// Find the path between start node and goal node using AStar Algorithm
    /// </summary>
    public static ArrayList FindPath(Node _start, Node _goal)
    {
        //Start Finding the path
        openList = new SortedList();
        openList.Add(_start);
        _start.Gcost = 0.0f;//set start gcost
        _start.Hcost = CostCalculation(_start, _goal);//set start node hcost
        _start.Fcost = _start.Gcost+ _start.Hcost;
        closedList = new HashSet<Node>();
        Node node = null;//init current node

        while (openList.Length != 0)
        {
            //set current node to first in list
            node = openList.First();
            //check if current node is the target node
            if (node.position == _goal.position)
            {
                return CalculatePath(node);
            }
            //Arraylist to store neighbor nodes
            ArrayList neighbors = new ArrayList();
            //Retrieve neighbors and store them in arraylist
            GridManager.Instance.GetNeighbors(node, neighbors);

            //Get the Neighbors
            for (int i = 0; i < neighbors.Count; i++)
            {
                //Cost between neighbor nodes
                Node neighborNode = (Node)neighbors[i];

                if (!closedList.Contains(neighborNode))
                {
                    //Cost from current node to this neighbor node
                    float cost = CostCalculation(node, neighborNode);

                    //Total Cost So Far from start to this neighbor node
                    float totalCost = node.Gcost + cost;

                    //Estimated cost for neighbor node to the goal
                    float neighbourNodeHCost = CostCalculation(neighborNode, _goal);

                    //Assign neighbour node properties
                    neighborNode.Gcost = totalCost;//Gcost
                    neighborNode.Hcost = neighbourNodeHCost;//Hcost
                    neighborNode.Fcost = totalCost + neighbourNodeHCost;//Fcost
                    neighborNode.parent = node;//set current node as parent
                    

                    //Add the neighbour node to the list if not already existed in the list
                    if (!openList.Contains(neighborNode))
                    {
                        openList.Add(neighborNode);
                    }
                }
            }
            //Add current node to closedList
            closedList.Add(node);
            //and remove it from openList
            openList.Remove(node);
        }

        //If finished looping and cannot find the goal then return null
        if (node.position != _goal.position)
        {
            Debug.LogError("Goal Not Found");
            return null;
        }

        //Calculate the path based on the final node
        return CalculatePath(node);
    }
}
