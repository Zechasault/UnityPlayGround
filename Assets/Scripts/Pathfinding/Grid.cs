using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    public Transform player, target;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        target = GetComponent<Pathfinding>().target;
        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector2 worldBottomLeft = new Vector2 (transform.position.x - gridWorldSize.x / 2, transform.position.y - gridWorldSize.y / 2);
        for(int i =0; i< gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                float pointX = worldBottomLeft.x + i * nodeDiameter + nodeRadius;
                float pointY = worldBottomLeft.y + j * nodeDiameter + nodeRadius;
                Vector2 worldPoint = new Vector2(pointX, pointY);
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeDiameter, unwalkableMask));
                    //!(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[i, j] = new Node(walkable, worldPoint, i, j);
            }
        }
    }

    public List<Node> GetNeighbors(Node n)
    {
        List<Node> neighbors = new List<Node>();
        for(int i=-1; i<=1; i++)
        {
            for(int j =-1; j<=1; j++)
            {
                if(i==0 & j == 0){ continue; }
                int checkI = n.gridX + i;
                int checkJ = n.gridY + j;
                if(checkI >= 0 && checkI < gridSizeX &&
                    checkJ >= 0 && checkJ < gridSizeY)
                {
                    neighbors.Add(grid[checkI, checkJ]);
                }
            }
        }
        return neighbors;
    }

    public Node GetNodeFromWorldPoint(Vector2 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        int i = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int j = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[i, j];
    }

    public List<Node> path;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1f));
        if (grid != null)
        {
            Node playerNode = GetNodeFromWorldPoint(player.position);
            Node targetNode = GetNodeFromWorldPoint(target.position);

            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if(path != null)
                {
                    if (path.Contains(n))
                    {
                        Gizmos.color = Color.blue;
                    }
                }
                if (n == playerNode)
                {
                    Gizmos.color = Color.green;
                }
                if (n == targetNode)
                {
                    Gizmos.color = Color.yellow;
                }
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter-0.01f));

            }
        }
    }

}
