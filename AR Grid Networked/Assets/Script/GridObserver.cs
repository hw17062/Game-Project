using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GridObserver : MonoBehaviourPun, IPunObservable
{
    public static GameObject LocalPlayerInstance;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Unit u = this.GetComponent<Unit>();
        int x = u.tileX;
        int z = u.tileZ;
        if (stream.IsWriting) //We own this player, send updates
        {
            stream.SendNext(x);
            stream.SendNext(z);
        }
        else //Network player, recieve updates
        {
            x = (int)stream.ReceiveNext();
            z = (int)stream.ReceiveNext();
            this.GetComponent<Unit>().tileX = x;
            this.GetComponent<Unit>().tileZ = z;
        }
    }

    void Awake()
    {
        if (photonView.IsMine)
        {
            GridObserver.LocalPlayerInstance = this.gameObject;
            GameObject map = GameObject.Find("Map");
            map.GetComponent<TileMap>().selectedUnit = gameObject;
            map.GetComponent<TileMap>().Init();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
