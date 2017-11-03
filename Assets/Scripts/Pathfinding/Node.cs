using System;
using System.Collections;
using UnityEngine;

public class Node : IHeapItem<Node>{
    public bool walkable;
    public Vector2 worldPosition;
    public int gCost, hCost;
    public int gridX, gridY;
    public Node parent;
    public int movementPenalty;
    int heapIndex;

    public Node(bool walkable, Vector2 worldPosition, int gridX, int gridY, int movementPenalty)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
        this.movementPenalty = movementPenalty;
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
