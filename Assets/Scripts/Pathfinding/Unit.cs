using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public Transform target;
    public float speed = 2f;

    Vector2[] path;
    int targetIndex;

    private void Start()
    {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    public void OnPathFound(Vector2[] newPath, bool pathSuccesfull)
    {
        if (pathSuccesfull)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector2 currentWaypoint = path[0];
        while (true)
        {
            if(new Vector2(transform.position.x, transform.position.y) == currentWaypoint)
            {
                targetIndex++;
                if(targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position,
                currentWaypoint, speed*Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for(int i=targetIndex; i<path.Length; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(path[i], new Vector3(0.1f,0.1f,0.1f));
                Gizmos.color = Color.black;
                if (i== targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
