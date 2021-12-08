using System;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Transform transform;
    public List<Node> children;

    public Node(Transform node)
    {
        this.transform = node;
        this.children = GetChildren();
    }

    private List<Node> GetChildren()
    {
        List<Node> children = new List<Node>();


        foreach (Transform child in transform)
        {
            Node childNode = new Node(child);

            children.Add(childNode);
        }

        return children;
    }


    public void Traverse(Action<Node> action)
    {
        action(this);

        foreach (Node child in children)
        {
            child.Traverse(action);
        }
    }

}