using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node:IComparable
{
    public int Gcost;//distance from start to current
    public int Hcost;//distance from current to final
    public int Fcost;//Fcost = Gcost + Hcost
    public Node parent;
    private bool isObstacle;
    public Node(bool _isobstacle = false)
    {
        Gcost = 1;
        Hcost = 0;
        parent = null;
        isObstacle = _isobstacle;
    }
    /// <summary>
    /// Sort an Array by Fcost
    /// </summary>
    /// <param name="obj">Node to compare to</param>
    /// <returns>Placement of Current Node in an Array</returns>
    public int CompareTo(object obj)
    {
        Node other = (Node)obj;
        //place current instance before comparing instance
        if (this.Fcost < other.Fcost)
        {
            return -1;
        }
        //place current instance after comparing instance
        else if(this.Fcost > other.Fcost)
        {
            return 1;
        }
        //dont move current instance
        return 0;
    }
}
