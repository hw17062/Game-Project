using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CubePlayerManager : MonoBehaviourPun, IPunObservable
{

    #region Public Fields

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    #endregion

    #region Private Fields

    [SerializeField]
    private Rigidbody rb;

    #endregion

    #region IPunObservable implementation


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Color32 col = GetComponent<MeshRenderer>().material.color;
        int r = col.r;
        int g = col.g;
        int b = col.b;
        int a = col.a;
        int pack = ((((((0 ^ r) << 8) ^ g) << 8) ^ b) << 8) ^ a;
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(pack);
        }
        else
        {
            // Network player, receive data
            pack = (int)stream.ReceiveNext();
            r = (pack >> 24)&0xFF;
            g = (pack >> 16) & 0xFF;
            b = (pack >> 8) & 0xFF;
            a = pack & 0xFF;
            GetComponent<MeshRenderer>().material.color = new Color32((Byte)r, (Byte)g, (Byte)b, (Byte)a);
        }
    }


    #endregion

    void Awake()
    {
        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (photonView.IsMine)
        {
            CubePlayerManager.LocalPlayerInstance = this.gameObject;
        }
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// </summary>
    void Start()
    {
        CubeCameraWork _cameraWork = this.gameObject.GetComponent<CubeCameraWork>();

        if (_cameraWork != null)
        {
            if (photonView.IsMine)
            {
                _cameraWork.OnStartFollowing();
            }
        }
        else
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }

        // When using trigger parameter
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * 250f);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Vector3 position = this.transform.position;
            position.x--;
            this.transform.position = position;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Vector3 position = this.transform.position;
            position.x++;
            this.transform.position = position;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Vector3 position = this.transform.position;
            position.z++;
            this.transform.position = position;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Vector3 position = this.transform.position;
            position.z--;
            this.transform.position = position;
        }
    }
}
