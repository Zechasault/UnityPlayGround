using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    public bool walkableGizmo, unwalkableGizmo;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public TerrainType[] walkableRegions;
    public int obstacleProximityPenalty = 10;
    LayerMask walkableMask;
    Node[,] grid;
    Dictionary<int, int> walkableRegionsDict = new Dictionary<int, int>();

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    int penaltyMin = int.MaxValue;
    int penaltyMax = int.MinValue;

    void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        foreach(TerrainType region in walkableRegions)
        {
            //unity's layers values are stored in binary, so if you want 2 layers, just need to OR (|) them
            // |= -> +=
            walkableMask.value += region.terrainMask.value;
            walkableRegionsDict.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty);
        }
        CreateGrid();
    }

    public int MaxSize { get
        {
            return gridSizeX * gridSizeY;
        }
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
                //!(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask)); //3D
                int movementPenalty = 0;
                //Need a 2D raycast, so a point in this case
                //Thus ensuring the ray's distance of 0
                //Obviously, it would be different if the world was in 3D.
                //A rain of rays coming from the sky (high z value)
                Ray ray = new Ray(new Vector3(worldPoint.x, worldPoint.y, 10f), Vector3.down);
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, 0f, walkableMask);
                if (hit)
                {
                    walkableRegionsDict.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                }
                if (!walkable)
                {
                    movementPenalty += obstacleProximityPenalty;
                }

                grid[i, j] = new Node(walkable, worldPoint, i, j, movementPenalty);
            }
        }
        BlurPenaltyGrid(3);
    }

    void BlurPenaltyGrid(int blurSize)
    {
        int kernelSize = blurSize * 2 + 1;
        int kernelExtents = (kernelSize-1) / 2;
        int[,] penaltiesHpath = new int[gridSizeX, gridSizeY];
        int[,] penaltiesVpath = new int[gridSizeX, gridSizeY];
        //horizontal
        for(int j =0;j<gridSizeY; j++)
        {
            for(int i=-kernelExtents; i<=kernelExtents; i++)
            {
                int sampleI = Mathf.Clamp(i, 0, kernelExtents);
                penaltiesHpath[0, j] += grid[sampleI, j].movementPenalty;
            }
            for(int i=1; i<gridSizeX; i++)
            {
                int removeIndex = Mathf.Clamp(i - kernelExtents - 1, 0, gridSizeX);
                int addIndex = Mathf.Clamp(i + kernelExtents,0,gridSizeX-1);
                penaltiesHpath[i, j] = penaltiesHpath[i - 1, j] - grid[removeIndex, j].movementPenalty + 
                    grid[addIndex, j].movementPenalty;
            }
        }
        //vertical
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = -kernelExtents; j <= kernelExtents; j++)
            {
                int sampleJ = Mathf.Clamp(j, 0, kernelExtents);
                penaltiesVpath[i, 0] += penaltiesHpath[i, sampleJ];
            }

            int blurredPenalty = Mathf.RoundToInt((float)penaltiesVpath[i, 0] / (kernelSize * kernelSize));
            grid[i, 0].movementPenalty = blurredPenalty;

            for (int j = 1; j < gridSizeY; j++)
            {
                int removeIndex = Mathf.Clamp(j - kernelExtents - 1, 0, gridSizeY);
                int addIndex = Mathf.Clamp(j + kernelExtents, 0, gridSizeY - 1);
                penaltiesVpath[i, j] = penaltiesHpath[i, j-1] - penaltiesHpath[i, removeIndex] +
                    penaltiesHpath[i, addIndex];
                //closest int
                blurredPenalty = Mathf.RoundToInt((float)penaltiesVpath[i, j] / (kernelSize * kernelSize));
                grid[i, j].movementPenalty = blurredPenalty;
                if(blurredPenalty > penaltyMax)
                {
                    penaltyMax = blurredPenalty;
                }
                if (blurredPenalty < penaltyMin)
                {
                    penaltyMin = blurredPenalty;
                }
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1f));
        if (grid != null)
        {
            foreach (Node n in grid)
            {
                if(n.walkable && walkableGizmo)
                {
                    Gizmos.color = Color.Lerp(Color.white, Color.black, 
                        Mathf.InverseLerp(penaltyMin, penaltyMax, n.movementPenalty));
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.01f));
                }
                else if (!n.walkable && unwalkableGizmo)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter /*- 0.01f*/));
                }
                
            }
        }
    }

    [Serializable]
    public class TerrainType
    {
        public LayerMask terrainMask;
        public int terrainPenalty;
    }
}
