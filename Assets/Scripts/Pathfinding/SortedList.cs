using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortedList
{
    private ArrayList list = new ArrayList();
    public int Length
    {
        get { return list.Count; }
    }
    public void Add(Node _node)
    {
        list.Add(_node);
        list.Sort();
    }
    public void Remove(Node _node)
    {
        list.Remove(_node);
        list.Sort();
    }
    public bool Contains(Node _node)
    {
        return list.Contains(_node);
    }
    public Node GetNode(int index)
    {
        return (Node)list[index];
    }
    public Node First()
    {
        if (list.Count > 0)
        {
            return (Node)list[0];
        }
        return null;
    }
}
