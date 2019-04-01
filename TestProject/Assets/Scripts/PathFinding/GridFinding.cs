using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This script is creating the Grid on the field by drawing cubes == nodes, which can be toggled on and off, also is setting the cubes are 
//showing the walkable and the unwalkable path with different colour. It is possible to resize the cube radius. The script also gives the neigbours of the cubes.
public class GridFinding : MonoBehaviour
{
    public bool DisplayGridGizmos; // showing the grid

    public LayerMask unwalkableMask; //setting the terrain that will not be walkable

    public Vector2 gridWorldSize; // size of the grid

    public float nodeRadius; //how big will the node be when it is drawn
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    //Getting the grid Maximum size
    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    //This function will create the grid, depending on the inputs that I have put, will also check if there are walkable, 
    //or unwalkable objects depending on the Layer Mask added before
    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for(int x = 0; x < gridSizeX ; x++)
        { 
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius,unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    //List that will get all neigbours so they can be used in the grid
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if(x == 0 && y ==0 )
                {
                    continue;
                }
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }

    //Function that creates the grid array, depending on the size of the grid, the array will increase or decrease
    public Node WorldNodePoint(Vector3 worldPosition) {
		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
		return grid[x,y];
	}

    //Function used to draw the nodes on the field, white nodes = walkable red nodes = unwalkable
    //The function will draw Wire cubes, with already set size, and position will be taken based on the grid size.
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 0.5f, gridWorldSize.y));
        if (grid != null && DisplayGridGizmos)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
        
    }
}