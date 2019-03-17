using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IComparable
{
    public float Gcost;//distance from start to current
    public float Hcost;//distance from current to final
    public float Fcost;//Fcost = Gcost + Hcost
    public bool isObstacle;
    public Node parent; 
    public Vector3 position;

    /// <summary>
    //Default Constructor
    /// </summary>
    public Node()
    {
        this.Hcost = 0.0f;
        this.Gcost = 1.0f;
        this.Fcost = 0.0f;
        this.isObstacle = false;
        this.parent = null;
    }
    /// <summary>
    //Constructor overload modifying Node position
    /// </summary>
    public Node(Vector3 _pos,bool _isobstacle = false)
    {
        this.Hcost = 0.0f;
        this.Gcost = 1.0f;
        this.Fcost = 0.0f;
        this.isObstacle = _isobstacle;
        this.parent = null;

        this.position = _pos;
    }
    /// <summary>
    ///Sorts the Nodes by Fcost;
    /// </summary>
    public int CompareTo(object obj)
    {
        Node node = (Node)obj;
        if (this.Fcost < node.Fcost)
            return -1;
        if (this.Fcost > node.Fcost)
            return 1;

        return 0;
    }
}
