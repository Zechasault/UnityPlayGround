using System;
using System.Collections;
using UnityEngine;

public class Node {
    public bool walkable {get; set;}
    public Vector2 worldPosition { get; set; }
    public int gCost { get; set; }
    public int hCost { get; set; }
    public int gridX { get; set; }
    public int gridY { get; set; }
    public Node parent { get; set; }

    public Node(bool walkable, Vector2 worldPosition, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public int fCost()
    {
        return gCost + hCost;
    }

    /*public int GetfCost()
    {
        return gCost + hCost;
    }

    public int GetgCost()
    {
        return gCost;
    }

    public int GethCost()
    {
        return hCost;
    }

    public int GetGridX()
    {
        return gridX;
    }

    public int GetGridY()
    {
        return gridY;
    }

    internal void SetgCost(int gCost)
    {
        this.gCost = gCost;
    }

    internal void SethCost(int hCost)
    {
        this.hCost = hCost;
    }

    internal void SetParent(Node parent)
    {
        this.parent = parent;
    }

    internal Node GetParent()
    {
        return parent;
    }*/
}
