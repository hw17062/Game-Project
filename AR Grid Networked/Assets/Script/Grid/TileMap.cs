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

    int mapSizeX = 20;
    int mapSizeZ = 30;

    GameObject ImageTarg;

    float offsetX;
    float offsetZ;

    // Start is called before the first frame update
    void Start()
    {
        ImageTarg = GameObject.Find("ImageTarget");

        offsetX = mapSizeX / 2;
        offsetZ = mapSizeZ / 2;


        ////selectedUnit.GetComponent<Unit>().tileX = (int)selectedUnit.transform.position.x;
        ////selectedUnit.GetComponent<Unit>().tileZ = (int)selectedUnit.transform.position.z;
        //selectedUnit.GetComponent<Unit>().map = this;

        //GenerateMapData();
        //GeneratePathfindingGraph();
        //GenerateMapVisual();
    }

    public void Init()
    {
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
        System.Random random = new System.Random();

        // Initialize our map tiles to be grass
        for (x = 0; x < mapSizeX; x++)
        {
            for (z = 0; z < mapSizeZ; z++)
            {
                int num = random.Next()%3;
                tiles[x, z] = num;
            }
        }

        //Make a lake/sea
        tiles[0, 23] = 5;
        tiles[0, 24] = 5;
        tiles[0, 25] = 5;
        tiles[0, 26] = 5;
        tiles[0, 27] = 5;
        tiles[0, 28] = 5;
        tiles[0, 29] = 5;

        tiles[1, 23] = 5;
        tiles[1, 24] = 5;
        tiles[1, 25] = 5;
        tiles[1, 26] = 5;
        tiles[1, 27] = 5;
        tiles[1, 28] = 5;
        tiles[1, 29] = 5;

        tiles[2, 24] = 5;
        tiles[2, 25] = 5;
        tiles[2, 26] = 5;
        tiles[2, 27] = 5;
        tiles[2, 28] = 5;
        tiles[2, 29] = 5;

        tiles[3, 25] = 5;
        tiles[3, 26] = 5;
        tiles[3, 27] = 5;
        tiles[3, 28] = 5;
        tiles[3, 29] = 5;

        tiles[4, 26] = 5;
        tiles[4, 27] = 5;
        tiles[4, 28] = 5;
        tiles[4, 29] = 5;

        tiles[5, 27] = 5;
        tiles[5, 28] = 5;
        tiles[5, 29] = 5;

        tiles[6, 28] = 5;
        tiles[6, 29] = 5;

        tiles[7, 29] = 5;

        //Make a river
        tiles[19, 2] = 5;
        tiles[19, 3] = 5;
        tiles[19, 2] = 5;
        tiles[19, 3] = 5;
        tiles[18, 2] = 5;
        tiles[18, 3] = 5;
        tiles[17, 2] = 5;
        tiles[17, 3] = 5;
        tiles[16, 2] = 5;
        tiles[16, 3] = 5;
        tiles[16, 4] = 5;
        tiles[15, 2] = 5;
        tiles[15, 3] = 5;
        tiles[15, 4] = 5;
        tiles[15, 5] = 5;
        tiles[15, 6] = 5;
        tiles[15, 7] = 5;
        tiles[14, 4] = 5;
        tiles[14, 5] = 5;
        tiles[14, 6] = 5;
        tiles[14, 7] = 5;
        tiles[14, 8] = 5;
        tiles[14, 9] = 5;
        tiles[14, 10] = 5;
        tiles[14, 11] = 5;
        tiles[14, 12] = 5;
        tiles[14, 13] = 5;
        tiles[13, 13] = 5;
        tiles[12, 13] = 5;
        tiles[11, 13] = 5;
        tiles[10, 13] = 5;
        tiles[9, 13] = 5;
        tiles[9, 14] = 5;
        tiles[9, 15] = 5;
        tiles[9, 16] = 5;
        tiles[9, 17] = 5;
        tiles[8, 17] = 5;
        tiles[8, 18] = 5;
        tiles[8, 19] = 5;
        tiles[8, 20] = 5;
        tiles[8, 21] = 5;
        tiles[8, 22] = 5;
        tiles[8, 23] = 5;
        tiles[7, 17] = 5;
        tiles[7, 18] = 5;
        tiles[7, 19] = 5;
        tiles[7, 20] = 5;
        tiles[7, 21] = 5;
        tiles[7, 22] = 5;
        tiles[7, 23] = 5;
        tiles[6, 22] = 5;
        tiles[6, 23] = 5;
        tiles[5, 22] = 5;
        tiles[5, 23] = 5;
        tiles[4, 22] = 5;
        tiles[4, 23] = 5;
        tiles[4, 24] = 5;
        tiles[4, 25] = 5;
        tiles[3, 22] = 5;
        tiles[3, 23] = 5;
        tiles[3, 24] = 5;


        //Make path
        tiles[16, 20] = 3;
        tiles[16, 19] = 3;
        tiles[16, 18] = 3;
        tiles[16, 17] = 3;
        tiles[15, 17] = 3;
        tiles[14, 17] = 3;
        tiles[13, 17] = 3;
        tiles[12, 17] = 3;
        tiles[12, 16] = 3;
        tiles[12, 15] = 3;
        tiles[11, 15] = 3;
        tiles[10, 15] = 3;


        //Make bridge
        tiles[9, 15] = 3;

        //Back to path
        tiles[8, 15] = 3;
        tiles[7, 15] = 3;
        tiles[6, 15] = 3;
        tiles[6, 14] = 3;
        tiles[6, 13] = 3;
        tiles[6, 12] = 3;
        tiles[6, 11] = 3;
        tiles[6, 10] = 3;
        tiles[6, 9] = 3;
        tiles[6, 8] = 3;
        tiles[6, 7] = 3;
        tiles[7, 8] = 3;
        tiles[5, 8] = 3;
        tiles[7, 7] = 3;
        tiles[5, 7] = 3;
        tiles[6, 6] = 3;
        tiles[7, 6] = 3;
        tiles[5, 6] = 3;


        //Placing trees
        tiles[11, 29] = 6;
        tiles[17, 5] = 7;
        tiles[18, 9] = 7;
        tiles[18, 11] = 7;
        tiles[17, 10] = 7;
        tiles[19, 17] = 7;
        tiles[19, 18] = 7;
        tiles[19, 19] = 7;
        tiles[18, 18] = 7;
        tiles[18, 19] = 7;
        tiles[0, 22] = 6;
        tiles[12, 0] = 7;
        tiles[14, 2] = 7;
        tiles[0, 13] = 7;
        tiles[1, 15] = 7;
        tiles[0, 17] = 7;

        //Stumps
        tiles[15, 15] = 8;
        tiles[13, 20] = 8;
        tiles[2, 21] = 8;
        tiles[12, 5] = 8;
        tiles[2, 11] = 8;
        tiles[6, 25] = 8;
        tiles[10, 25] = 8;

        //Rocks
        tiles[19, 0] = 11;
        tiles[19, 1] = 12;
        tiles[19, 4] = 11;
        tiles[18, 1] = 11;
        tiles[7, 29] = 12;

        //Bush
        tiles[18, 4] = 10;
        tiles[17, 1] = 10;
        tiles[13, 9] = 9;
        tiles[5, 13] = 9;
        tiles[6, 18] = 10;
        tiles[9, 21] = 9;
        tiles[14, 0] = 10;
        tiles[2, 12] = 9;
        tiles[2, 18] = 9;
        tiles[0, 20] = 10;
        tiles[8, 29] = 9;
        tiles[11, 18] = 10;

        // Make castle area non-walkable
        tiles[12, 21] = 3;
        tiles[13, 21] = 3;
        tiles[14, 21] = 3;
        tiles[15, 21] = 3;
        tiles[16, 21] = 3;
        tiles[17, 21] = 3;
        tiles[18, 21] = 3;
        tiles[19, 21] = 3;

        tiles[12, 22] = 3;
        tiles[13, 22] = 4;
        tiles[14, 22] = 4;
        tiles[15, 22] = 4;
        tiles[16, 22] = 4;
        tiles[17, 22] = 4;
        tiles[18, 22] = 4;
        tiles[19, 22] = 4;

        tiles[12, 23] = 3;
        tiles[13, 23] = 4;
        tiles[14, 23] = 4;
        tiles[15, 23] = 4;
        tiles[16, 23] = 4;
        tiles[17, 23] = 4;
        tiles[18, 23] = 4;
        tiles[19, 23] = 4;

        tiles[12, 24] = 3;
        tiles[13, 24] = 4;
        tiles[14, 24] = 4;
        tiles[15, 24] = 4;
        tiles[16, 24] = 4;
        tiles[17, 24] = 4;
        tiles[18, 24] = 4;
        tiles[19, 24] = 4;

        tiles[12, 25] = 3;
        tiles[13, 25] = 4;
        tiles[14, 25] = 4;
        tiles[15, 25] = 4;
        tiles[16, 25] = 4;
        tiles[17, 25] = 4;
        tiles[18, 25] = 4;
        tiles[19, 25] = 4;

        tiles[12, 26] = 3;
        tiles[13, 26] = 4;
        tiles[14, 26] = 4;
        tiles[15, 26] = 4;
        tiles[16, 26] = 4;
        tiles[17, 26] = 4;
        tiles[18, 26] = 4;
        tiles[19, 26] = 4;

        tiles[12, 27] = 3;
        tiles[13, 27] = 4;
        tiles[14, 27] = 4;
        tiles[15, 27] = 4;
        tiles[16, 27] = 4;
        tiles[17, 27] = 4;
        tiles[18, 27] = 4;
        tiles[19, 27] = 4;

        tiles[12, 28] = 3;
        tiles[13, 28] = 4;
        tiles[14, 28] = 4;
        tiles[15, 28] = 4;
        tiles[16, 28] = 4;
        tiles[17, 28] = 4;
        tiles[18, 28] = 4;
        tiles[19, 28] = 4;

        tiles[12, 29] = 3;
        tiles[13, 29] = 3;
        tiles[14, 29] = 3;
        tiles[15, 29] = 3;
        tiles[16, 29] = 3;
        tiles[17, 29] = 3;
        tiles[18, 29] = 3;
        tiles[19, 29] = 3;

        //Make houses and fence non-walkable
        tiles[0, 0] = 13;
        tiles[0, 1] = 13;
        tiles[0, 2] = 13;
        tiles[0, 3] = 13;
        tiles[0, 4] = 13;
        tiles[0, 5] = 13;
        tiles[0, 6] = 13;
        tiles[0, 7] = 13;
        tiles[0, 8] = 13;
        tiles[0, 9] = 13;
        tiles[0, 10] = 13;

        tiles[1, 0] = 13;
        tiles[1, 3] = 13;
        tiles[1, 4] = 13;
        tiles[1, 5] = 13;
        tiles[1, 6] = 13;
        tiles[1, 7] = 13;
        tiles[1, 8] = 13;
        tiles[1, 9] = 13;
        tiles[1, 10] = 13;

        tiles[2, 0] = 13;
        tiles[2, 3] = 13;
        tiles[2, 4] = 13;
        tiles[2, 5] = 13;
        tiles[2, 6] = 13;
        tiles[2, 7] = 13;
        tiles[2, 8] = 13;
        tiles[2, 9] = 13;
        tiles[2, 10] = 13;

        tiles[3, 0] = 13;
        tiles[3, 3] = 13;
        tiles[3, 4] = 13;
        tiles[3, 5] = 13;
        tiles[3, 7] = 13;
        tiles[3, 8] = 13;
        tiles[3, 9] = 13;
        tiles[3, 10] = 13;

        tiles[4, 0] = 13;
        tiles[4, 1] = 13;
        tiles[4, 2] = 13;
        tiles[4, 10] = 13;

        tiles[5, 0] = 13;
        tiles[5, 1] = 13;
        tiles[5, 2] = 13;

        tiles[6, 0] = 13;
        tiles[6, 1] = 13;
        tiles[6, 2] = 13;

        tiles[7, 0] = 13;

        tiles[8, 0] = 13;
        tiles[8, 3] = 13;
        tiles[8, 4] = 13;
        tiles[8, 5] = 13;
        tiles[8, 10] = 13;

        tiles[9, 0] = 13;
        tiles[9, 3] = 13;
        tiles[9, 4] = 13;
        tiles[9, 5] = 13;
        tiles[9, 10] = 13;

        tiles[10, 0] = 13;
        tiles[10, 1] = 13;
        tiles[10, 2] = 13;
        tiles[10, 3] = 13;
        tiles[10, 4] = 13;
        tiles[10, 5] = 13;
        tiles[10, 6] = 13;
        tiles[10, 7] = 13;
        tiles[10, 8] = 13;
        tiles[10, 9] = 13;
        tiles[10, 10] = 13;

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
            for (int z = 0; z < mapSizeZ; z++)
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
                GameObject go = (GameObject)Instantiate(tt.tileVisualPrefab, new Vector3(x-offsetX, 0, z-offsetZ), tt.tileVisualPrefab.transform.rotation);

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
        if(tiles[x, z] == 5)
            return floor[x, z].transform.position + new Vector3(0, 0.5f, 0);
        else 
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
