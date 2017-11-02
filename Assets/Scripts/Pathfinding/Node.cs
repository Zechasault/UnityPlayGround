using System;
using System.Collections;
using UnityEngine;

public class Node : IHeapItem<Node>{
    public bool walkable {get; set;}
    public Vector2 worldPosition { get; set; }
    public int gCost { get; set; }
    public int hCost { get; set; }
    public int gridX { get; set; }
    public int gridY { get; set; }
    public Node parent { get; set; }
    int heapIndex;

    public Node(bool walkable, Vector2 worldPosition, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public int fCost
    {
        get{
            return gCost + hCost;
        }
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node n)
    {
        int compare = fCost.CompareTo(n.fCost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(n.hCost);
        }
        //Nodes are reversed !
        return -compare;
    }
}
