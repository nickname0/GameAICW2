using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script will be added to the Enemy, so he will be able to get the player position always
//By this scrit also he will refresh the player position and will find the fastest way to him
public class Unit : MonoBehaviour
{
    Vector3 pos;

    const float minpathUpdateTime = 0.2f;

    const float pathUpdateMoveThreshold = 0.5f;

    public Transform target;
    public float speed = 1;
    Vector3[] path;
    int targetIndex = 0;

    void Start()
    {
        pos = new Vector3(Random.Range(-8.5f, 8.5f), 0.25f, Random.Range(-4.5f, 5));
        transform.position = pos;

        StartCoroutine("UpdatePath");
    }

    //function that will check if the path is the correct one
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }
    // If the player move to somewhere the path will change with it 
    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < 0.3f) {
            yield return new WaitForSeconds(0.3f);
            }
        RequestManager.RequestPath(transform.position, target.position, OnPathFound); // This will be the first possition of the player
            
        float sqrMoveTreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = target.position;

        while(true)
        {
            yield return new WaitForSeconds(minpathUpdateTime);
            if ((target.position - targetPosOld).sqrMagnitude > sqrMoveTreshold)
            {
                RequestManager.RequestPath(transform.position, target.position, OnPathFound); // here I will update the player position
                targetPosOld = target.position; //getting the old position
            }
        }
    }

    //This function wil make the enemy(Blue ball) to follow the path by using the nodes passed to it
    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }


    //This function will create the black boxes, which will be connected with lines, that shows up the quickest way to the Target(player)
    public void OnDrawGizmos()
    {
        if(path !=null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], new Vector3(0.1f,0.1f,0.1f));

                if (i ==targetIndex)
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
