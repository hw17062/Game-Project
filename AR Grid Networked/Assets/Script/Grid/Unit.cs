using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Unit : MonoBehaviourPun
{
    public int tileX;
    public int tileZ;
    public TileMap map;
    public List<Node> currentPath = null;

    public int moveSpeed = 100000;
    public float remainingMovement = 100000;
    GameObject ImageTarg;

    private void Awake()
    {
        ImageTarg = GameObject.Find("ImageTarget");
        this.transform.SetParent(ImageTarg.transform, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (currentPath != null)
            {
                int currNode = 0;

                while (currNode < currentPath.Count - 1)
                {
                    Vector3 start = map.TileCoordToWorldCoord(currentPath[currNode].x, currentPath[currNode].z) +
                        new Vector3(0, -0.5f, 0);
                    Vector3 end = map.TileCoordToWorldCoord(currentPath[currNode + 1].x, currentPath[currNode + 1].z) +
                        new Vector3(0, -0.5f, 0);

                    currNode++;
                }
            }

            if (Vector3.Distance(transform.position, map.TileCoordToWorldCoord(tileX, tileZ)) < 0.1f)
                AdvancePathing();

            transform.position = Vector3.Lerp(transform.position, map.TileCoordToWorldCoord(tileX, tileZ), 5f * Time.deltaTime);

        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, GameObject.Find("Map").GetComponent<TileMap>().TileCoordToWorldCoord(tileX, tileZ), 5f * Time.deltaTime);
        }
    }

    void AdvancePathing()
    {
        if (currentPath == null)
            return;

        if (remainingMovement <= 0)
            return;

        transform.position = map.TileCoordToWorldCoord(tileX, tileZ);

        remainingMovement -= map.CostToEnterTile(currentPath[0].x, currentPath[0].z, currentPath[1].x, currentPath[1].z);

        tileX = currentPath[1].x;
        tileZ = currentPath[1].z;

        currentPath.RemoveAt(0);

        if (currentPath.Count == 1)
            currentPath = null;
    }

    public void NextTurn()
    {
        while (currentPath != null && remainingMovement > 0)
        {
            AdvancePathing();
        }

        remainingMovement = moveSpeed;
    }
}
