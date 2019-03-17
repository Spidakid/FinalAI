using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortedList
{
    // node array to store the priority queue
    private ArrayList nodes = new ArrayList();

    /// <summary>
    /// Number of nodes in the priority queue
    /// </summary>
    public int Length
    {
        get { return this.nodes.Count; }
    }

    /// <summary>
    /// Check whether the node is already in the queue or not
    /// </summary>
    public bool Contains(Node _node)
    {
        return this.nodes.Contains(_node);
    }

    /// <summary>
    /// Get the first node in the queue
    /// </summary>
    public Node First()
    {
        if (this.nodes.Count > 0)
        {
            return (Node)this.nodes[0];
        }
        return null;
    }

    /// <summary>
    /// Add the node to the priority queue and sort with the estimated total cost
    /// </summary>
    public void Add(Node _node)
    {
        this.nodes.Add(_node);
        this.nodes.Sort();
    }

    /// <summary>
    /// Add the node from the priority queue and sort the remaining with the estimated total cost
    /// </summary>
    public void Remove(Node _node)
    {
        this.nodes.Remove(_node);
        this.nodes.Sort();
    }
}
