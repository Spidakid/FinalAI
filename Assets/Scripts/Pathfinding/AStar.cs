using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    public static SortedList openList;
    public static HashSet<Node> closedList;
    /// <summary>
    /// Cost Calculation
    /// </summary>
    /// <param name="curNode"></param>
    /// <param name="goalNode"></param>
    /// <returns></returns>
    private static float CostCalculation(Node curNode,Node goalNode)
    {
        Vector3 vecCost = curNode.position - goalNode.position;
        return vecCost.magnitude;
    }
    private static ArrayList FindPath(Node _start,Node _goal)
    {
        openList = new SortedList();
        openList.Add(_start);
        _start.Gcost = 0.0f;//set start node gcost
        _start.Hcost = CostCalculation(_start,_goal);//set start node hcost
        _start.Fcost = _start.Gcost + _start.Hcost;

        closedList = new HashSet<Node>();
        //initialize current node
        Node node = null;
        //Finding a path loop
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
            //Find a neighbor with the least Fcost
            for (int i = 0; i < neighbors.Count; i++)
            {
                Node neighborNode = (Node)neighbors[i];
                //Checks if node is already in closedList
                if (!closedList.Contains(neighborNode))
                {
                    //Cost from current to neighbor node
                    float cost = CostCalculation(node,neighborNode);
                    float totalCost = node.Gcost + cost;//Gcost of currentnode + Gcost
                    float neighborNodeHCost = CostCalculation(neighborNode,_goal);//neighbor Hcost

                    neighborNode.Gcost = totalCost;//neighbor Gcost
                    neighborNode.Hcost = neighborNodeHCost;//neighbor Hcost
                    //*Set current node as the parent
                    neighborNode.parent = node;
                    neighborNode.Fcost = totalCost + neighborNodeHCost; //neighbor Fcost
                    //Check if current neighbor is in the open list
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
        if (node.position !=_goal.position)
        {
            Debug.LogError("Goal Not Found!");
            return null;
        }
        return CalculatePath(node);
    }
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
        list.Reverse();//Since we want a path array from the start node to the target node, we call this method;
        return list;
    }
}
