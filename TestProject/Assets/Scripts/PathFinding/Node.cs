using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem <Node>
{
    
    public bool walkable;

    //Position of the nodes
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    //Costs for move
    public int gCost; //distance from starting node
    public int hCost; //distance from end node
    public Node parent;
    int heapIndex;

    //checking if the node is walkable
    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    // finding the cost of the moves
    public int fCost // G cost + H Cost
    {
        get
        {
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

    //Comparing nodes, will always take the lowest fCost node
    public int CompareTo(Node nodeToCompare) //Compare the F cost of 2 nodes
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
