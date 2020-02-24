using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    public GameObject selectedUnit;

    public TileType[] tileTypes;

    int[,] tiles;
    Node[,] graph;
    GameObject[,] floor;

    int mapSizeX = 10;
    int mapSizeZ = 10;

    GameObject ImageTarg;

    float offsetX;
    float offsetZ;

    // Start is called before the first frame update
    void Start()
    {
        //ImageTarg = GameObject.Find("ImageTarget");

        //offsetX = mapSizeX / 2;
        //offsetZ = mapSizeZ / 2;


        ////selectedUnit.GetComponent<Unit>().tileX = (int)selectedUnit.transform.position.x;
        ////selectedUnit.GetComponent<Unit>().tileZ = (int)selectedUnit.transform.position.z;
        //selectedUnit.GetComponent<Unit>().map = this;

        //GenerateMapData();
        //GeneratePathfindingGraph();
        //GenerateMapVisual();
    }

    public void Init()
    {
        ImageTarg = GameObject.Find("ImageTarget");

        offsetX = mapSizeX / 2;
        offsetZ = mapSizeZ / 2;
        //ImageTarg = GameObject.Find("ImageTarget");

        //offsetX = mapSizeX / 2;
        //offsetZ = mapSizeZ / 2;


        //selectedUnit.GetComponent<Unit>().tileX = (int)selectedUnit.transform.position.x;
        //selectedUnit.GetComponent<Unit>().tileZ = (int)selectedUnit.transform.position.z;
        selectedUnit.GetComponent<Unit>().map = this;

        GenerateMapData();
        GeneratePathfindingGraph();
        GenerateMapVisual();
    }

    void GenerateMapData()
    {
        tiles = new int[mapSizeX, mapSizeZ];

        int x, z;

        // Initialize our map tiles to be grass
        for (x = 0; x < mapSizeX; x++)
        {
            for (z = 0; z < mapSizeZ; z++)
            {
                tiles[x, z] = 0;
            }
        }

        // Make a big swamp area
        for (x = 0; x <= 5; x++)
        {
            for (z = 0; z < 4; z++)
            {
                tiles[x, z] = 1;
            }
        }

        // Make a mountain range
        tiles[4, 4] = 2;
        tiles[5, 4] = 2;
        tiles[6, 4] = 2;
        tiles[7, 4] = 2;
        tiles[8, 4] = 2;

        tiles[4, 5] = 2;
        tiles[4, 6] = 2;
        tiles[8, 5] = 2;
        tiles[8, 6] = 2;
    }

    public float CostToEnterTile(int sourceX, int sourceZ, int targetX, int targetZ)
    {

        TileType tt = tileTypes[tiles[targetX, targetZ]];

        if (UnitCanEnterTile(targetX, targetZ) == false)
            return Mathf.Infinity;

        float cost = tt.movementCost;

        if (sourceX != targetX && sourceZ != targetZ)
        {
            cost += 0.001f;
        }

        return cost;

    }

    void GeneratePathfindingGraph()
    {
        // Initialize the array
        graph = new Node[mapSizeX, mapSizeZ];

        // Initialize a Node for each spot in the array
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int z = 0; z < mapSizeZ; z++)
            {
                graph[x, z] = new Node();
                graph[x, z].x = x;
                graph[x, z].z = z;
            }
        }

        //Calculate nodes neighbours
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int z = 0; z < mapSizeX; z++)
            {

                // Try left
                if (x > 0)
                {
                    graph[x, z].neighbours.Add(graph[x - 1, z]);
                    if (z > 0)
                        graph[x, z].neighbours.Add(graph[x - 1, z - 1]);
                    if (z < mapSizeZ - 1)
                        graph[x, z].neighbours.Add(graph[x - 1, z + 1]);
                }

                // Try Right
                if (x < mapSizeX - 1)
                {
                    graph[x, z].neighbours.Add(graph[x + 1, z]);
                    if (z > 0)
                        graph[x, z].neighbours.Add(graph[x + 1, z - 1]);
                    if (z < mapSizeZ - 1)
                        graph[x, z].neighbours.Add(graph[x + 1, z + 1]);
                }

                // Try straight up and down
                if (z > 0)
                    graph[x, z].neighbours.Add(graph[x, z - 1]);
                if (z < mapSizeZ - 1)
                    graph[x, z].neighbours.Add(graph[x, z + 1]);

            }
        }
    }

    void GenerateMapVisual()
    {
        float offsetX = mapSizeX / 2;
        float offsetZ = mapSizeZ / 2;
        floor = new GameObject[mapSizeX, mapSizeZ];
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int z = 0; z < mapSizeZ; z++)
            {
                TileType tt = tileTypes[tiles[x, z]];
                GameObject go = (GameObject)Instantiate(tt.tileVisualPrefab, new Vector3(x - offsetX, 0, z - offsetZ), Quaternion.identity);
                
                go.transform.SetParent(ImageTarg.transform);

                ClickableTile ct = go.GetComponent<ClickableTile>();
                ct.tileX = x;
                ct.tileZ = z;
                ct.map = this;
                floor[x, z] = go;
            }
        }
    }

    public Vector3 TileCoordToWorldCoord(int x, int z)
    {
        //return new Vector3(x, 0, z);
        //return ImageTarg.transform.position + new Vector3(x - offsetX, 1, z - offsetZ);
        return floor[x, z].transform.position + new Vector3(0, 1, 0);
    }

    public GameObject NewLoc(int x, int z)
    {
        return floor[x, z];
    }

    public bool UnitCanEnterTile(int x, int z)
    {

        return tileTypes[tiles[x, z]].isWalkable;
    }

    public void GeneratePathTo(int x, int z)
    {
        // Clear out unit's old path.
        selectedUnit.GetComponent<Unit>().currentPath = null;

        if (UnitCanEnterTile(x, z) == false)
        {
            return;
        }

        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();


        List<Node> unvisited = new List<Node>();

        Node source = graph[selectedUnit.GetComponent<Unit>().tileX, selectedUnit.GetComponent<Unit>().tileZ];

        Node target = graph[x, z];

        dist[source] = 0;
        prev[source] = null;

        // Initialize everything to have infinite distance
        foreach (Node v in graph)
        {
            if (v != source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }

            unvisited.Add(v);
        }

        while (unvisited.Count > 0)
        {
            //the unvisited node with the smallest distance.
            Node u = null;

            foreach (Node possibleU in unvisited)
            {
                if (u == null || dist[possibleU] < dist[u])
                {
                    u = possibleU;
                }
            }

            if (u == target)
            {
                break;  // Exit the while loop!
            }

            unvisited.Remove(u);

            foreach (Node v in u.neighbours)
            {
                float alt = dist[u] + CostToEnterTile(u.x, u.z, v.x, v.z);
                if (alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }


        if (prev[target] == null)
        {
            // No route between target and source
            return;
        }

        List<Node> currentPath = new List<Node>();

        Node curr = target;

        // Step through the "prev" chain and add it to path
        while (curr != null)
        {
            currentPath.Add(curr);
            curr = prev[curr];
        }


        currentPath.Reverse();

        selectedUnit.GetComponent<Unit>().currentPath = currentPath;
    }
}
