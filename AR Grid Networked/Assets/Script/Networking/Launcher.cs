using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;


public class Launcher : MonoBehaviourPunCallbacks
{

    #region Private Serializable Fields

    /// <summary>
    /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
    /// </summary>
    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField]
    private byte maxPlayersPerRoom = 6;

    //[SerializeField]
    //GameObject gameManagerPrefab;

    #endregion

    #region Private Fields

    /// <summary>
    /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
    /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
    /// Typically this is used for the OnConnectedToMaster() callback.
    /// </summary>
    bool isConnecting;
    bool isActive = true;
    //bool alreadyScanning = false;

    //ScanCards scan;

    #endregion

    #region Event Codes
    //THIS REGION NEEDS TO BE THE SAME ACROSS ALL SCRIPTS THAT USE EVENT HANDLING
    //CURRENTLY: Launcher.cs, GameManager.cs in ARGRID and GameManager.cs in master client

    const byte kickCode = 1;
    const byte acceptPlayerCode = 2;
    const byte moveCode = 3;
    const byte msgCode = 4;

    #endregion

    //public GameObject cam;
    public GameObject canv;
    public GameObject connectingCanv;
    //public GameObject dirLight;
    public GameObject events;
    //public GameObject gameManager;

    //public Camera ar;
    //public Camera qr;

    //#region Public Fields

    //[Tooltip("The Ui Panel to let the user enter name, connect and play")]
    //[SerializeField]
    //private GameObject controlPanel;
    //[Tooltip("The UI Label to inform the user that the connection is in progress")]
    //[SerializeField]
    //private GameObject progressLabel;

    //#endregion

    /// <summary>
    /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
    /// </summary>
    string gameVersion = "1";

    #region MonoBehaviorPunCallbacks Callbacks

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (!isActive)
        {
            return;
        }
        switch (eventCode)
        {
            case acceptPlayerCode:
                Debug.Log("Accepted");
                //Destroy(cam);
                //Destroy(dirLight);
                //Destroy(canv);
                //Destroy(connectingCanv);
                //Destroy(events);
                canv.SetActive(false);
                connectingCanv.SetActive(false);
                events.SetActive(false);
                Instantiate(Resources.Load("GameManager"), new Vector3(0,0,0), new Quaternion(0,0,0,0));
                isActive = false;
                //SceneManager.LoadScene("ARGRID", LoadSceneMode.Additive);
                //Destroy(gameObject);
                break;
            case kickCode:
                Debug.Log("Rejected");
                canv.SetActive(true);
                connectingCanv.SetActive(false);
                //Add message here, which classes are free? Is room full?
                break;
        }

        //if (eventCode == acceptPlayerCode)
        //{
        //    Destroy(cam);
        //    Destroy(dirLight);
        //    Destroy(canv);
        //    Destroy(events);
        //    SceneManager.LoadScene("ARGRID", LoadSceneMode.Additive);
        //    Destroy(gameObject);
        //}

        //if (eventCode == MoveUnitsToTargetPositionEvent)
        //{
        //    object[] data = (object[])photonEvent.CustomData;

        //    Vector3 targetPosition = (Vector3)data[0];

        //    for (int index = 1; index < data.Length; ++index)
        //    {
        //        int unitId = (int)data[index];

        //        UnitList[unitId].TargetPosition = targetPosition;
        //    }
        //}
    }

    public override void OnEnable()
    {
        //Without the base calls, the photon callbacks don't work.
        //Need to override base OnEnable to allow for event handling
        //Event handling will be how communication from master client to payer client will be achieved
        base.OnEnable(); //VERY IMPORTANT DO NOT REMOVE
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    public override void OnDisable()
    {
        base.OnDisable(); //NOT EVEN THIS ONE
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
        //Debug.Log("Called");
        // we don't want to do anything if we are not attempting to join a room.
        // this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
        // we don't want to do anything.
        if (isConnecting)
        {
            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
            PhotonNetwork.JoinRandomRoom();
        }
    }

    //public override void OnConnectedToMasterFailed()
    //{

    //}

    //public override void OnDisconnected(DisconnectCause cause)
    //{
    //    progressLabel.SetActive(false);
    //    controlPanel.SetActive(true);
    //    Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    //}

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        //PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        Connect();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        // #Critical: We only load if we are the first player, else we rely on `PhotonNetwork.AutomaticallySyncScene` to sync our instance scene.
        //if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        //{
        //    Debug.Log("We load the 'Room for 1' ");


        //    // #Critical
        //    // Load the Room Level.
        //    PhotonNetwork.LoadLevel("Room for 1");
        //}
        //Instantiate(gameManagerPrefab);
        //Destroy(cam);
        //Destroy(dirLight);
        //Destroy(canv);
        //Destroy(connectingCanv);
        //Destroy(events);
        //SceneManager.LoadScene("ARGRID", LoadSceneMode.Additive);
        //Destroy(gameObject);
    }

    #endregion

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// </summary>
    void Awake()
    {
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        //PhotonNetwork.AutomaticallySyncScene = true;
        //cam = GameObject.Find("ARCamera");
        //qr.enabled = false;
        //ar.enabled = true;
        //canv = GameObject.Find("Canvas");
        //scan = qr.GetComponent<ScanCards>();
        connectingCanv.SetActive(false);
    }


    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// </summary>
    void Start()
    {
        //progressLabel.SetActive(false);
        //controlPanel.SetActive(true);
        //PhotonNetwork.NickName = "Warrior";
        //Connect();
        //PhotonNetwork.ConnectUsingSettings();
        //AutoJoin thing
    }

    private void Update()
    {
        //if (!alreadyScanning && scan.CamReady)
        //{
        //    alreadyScanning = true;
        //    scan.RegisterPlayer(this);
        //}
    }


    /// <summary>
    /// Start the connection process.
    /// - If already connected, we attempt joining a random room
    /// - if not yet connected, Connect this application instance to Photon Cloud Network
    /// </summary>
    public void Connect()
    {
        // keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
        isConnecting = true;
        connectingCanv.SetActive(true);
        canv.SetActive(false);
        //progressLabel.SetActive(true);
        //controlPanel.SetActive(false);
        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void ClassSelected(string job)
    {
        PhotonNetwork.NickName = job;
        Connect();
    }

    //public void scanResult(string job)
    //{
    //    if (job.Equals("fail"))
    //    {
    //        alreadyScanning = false;
    //        return;
    //    }
    //    PhotonNetwork.NickName = job;
    //    ar.enabled = true;
    //    qr.enabled = false;
    //    Destroy(canv);
    //    Destroy(qr.gameObject);
    //    Connect();
    //}


}
