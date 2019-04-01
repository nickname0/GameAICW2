using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;


//A* pathfinding algorithm this script is used to start the movement of the enemy towards the player it is retracable so when the player position change 
//the path will change with it. It is finding the fastest way possible.
public class Pathfinding : MonoBehaviour
{
    RequestManager manager;

    GridFinding grid;

    void Awake()
    {
        manager = GetComponent<RequestManager>();
        grid = GetComponent<GridFinding>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos) // In this fucntion the player finds the path to the finish position
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.WorldNodePoint(startPos); // This is the starting position
        Node targetNode = grid.WorldNodePoint(targetPos); // This is the finish position

        if (startNode.walkable && targetNode.walkable)
        {

            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);                //Creating new empty arrays
            HashSet<Node> closedSet = new HashSet<Node>();        //Creating new empty arrays
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node node = openSet.Remove();
                closedSet.Add(node); // adding the closest node

                if (node == targetNode)
                {
                    sw.Stop();
                    print("Path found: " + sw.ElapsedMilliseconds + " ms"); //Showing for what time the enemy find the fastest path to the player
                    pathSuccess = true;
                    break;
                }

                foreach (Node neigbour in grid.GetNeighbours(node))
                {
                    if (!neigbour.walkable || closedSet.Contains(neigbour)) //checking if the neigbour is walkable
                    {
                        continue;
                    }

                    int newMoveCostToNeigh = node.gCost + GetDistance(node, neigbour);
                    if (newMoveCostToNeigh < neigbour.gCost || !openSet.Contains(neigbour)) //Checking if the neigbour node is not in the openSet 
                    {
                        neigbour.gCost = newMoveCostToNeigh;
                        neigbour.hCost = GetDistance(neigbour, targetNode);
                        neigbour.parent = node;

                        if (!openSet.Contains(neigbour))// If the neighbour is not in the openSet he will be added
                        {
                            openSet.Add(neigbour);
                        }
                        else
                        {
                            openSet.UpdateItem(neigbour);
                        }
                    }
                }
            }
        }
        yield return null; // will wait for a frame before returning 

        //if this is true the RetracePath will be called and a new way will be find 
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
            pathSuccess = waypoints.Length > 0;
        }

        manager.FinishedProcessingPath(waypoints,pathSuccess);

    }

    Vector3[] RetracePath(Node startNode, Node endNode) // When the player position change, the path that will be closest to the finish will change also
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);

        return waypoints;
    }

    //
    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i<path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i - 1].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    int GetDistance(Node nodeA,Node nodeB) // function that will get the fastest way possible to the finish
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX); //Distance X
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY); //Distance Y

        if (dstX>dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }

        return 14 * dstX + 10 * (dstY - dstX);
    }
}
