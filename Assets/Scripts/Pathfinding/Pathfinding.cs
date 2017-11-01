using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {
    Grid grid;
    public Transform seeker, target;
    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Update()
    {
        FindPath(seeker.position, target.position);
    }

    void FindPath(Vector2 startPos, Vector2 targetPos)
    {
        Node startNode = grid.GetNodeFromWorldPoint(startPos);
        Node targetNode = grid.GetNodeFromWorldPoint(targetPos);
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);
        while(openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for(int i=1; i<openSet.Count; i++)
            {
                if(openSet[i].fCost() < currentNode.fCost() ||
                    openSet[i].fCost() == currentNode.fCost() &&
                    openSet[i].fCost() == currentNode.fCost())
                {
                    currentNode = openSet[i];
                }
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            if(currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }
            foreach(Node neighbor in grid.GetNeighbors(currentNode))
            {
                if(!neighbor.walkable || closedSet.Contains(neighbor))
                {
                    continue;
                }
                int newMvtCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if(newMvtCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMvtCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        grid.path = path;
    }

    int GetDistance(Node n1, Node n2)
    {
        int dstX = Mathf.Abs(n1.gridX - n2.gridX);
        int dstY = Mathf.Abs(n1.gridY - n2.gridY);
        if(dstX > dstY)
        {
            //the distance between neighbors horizontally equals 10
            //so the distance between neighbors diagonnaly equals 14 (sqrt(2)*10)
            return 14 * dstY + 10 * (dstX - dstY);
        }
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
