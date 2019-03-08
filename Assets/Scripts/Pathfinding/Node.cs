using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node:IComparable
{
    public float Gcost;//distance from start to current
    public float Hcost;//distance from current to final
    public float Fcost;//Fcost = Gcost + Hcost
    public Node parent;
    public bool isObstacle;
    public Vector3 position;
    public Node(Vector3 _pos,bool _isobstacle = false)
    {
        Gcost = 1f;
        Hcost = 0f;
        parent = null;
        isObstacle = _isobstacle;
        position = _pos;
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
