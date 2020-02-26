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

    #endregion

    #region Private Fields

    /// <summary>
    /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
    /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
    /// Typically this is used for the OnConnectedToMaster() callback.
    /// </summary>
    bool isConnecting;
    bool isActive = true;

    #endregion

    #region Event Codes
    //THIS REGION NEEDS TO BE THE SAME ACROSS ALL SCRIPTS THAT USE EVENT HANDLING
    //CURRENTLY: Launcher.cs, GameManager.cs in ARGRID and GameManager.cs in master client

    const byte kickCode = 1;
    const byte acceptPlayerCode = 2;
    const byte moveCode = 3;
    const byte msgCode = 4;

    #endregion

    public GameObject canv;
    public GameObject connectingCanv;
    public GameObject events;

    /// <summary>
    /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
    /// </summary>
    string gameVersion = "1";

    #region MonoBehaviorPunCallbacks Callbacks


    //Event Handling for accepted to room, and kicked from room
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
                //Join request was accepted, finish joining
                Debug.Log("Accepted");
                canv.SetActive(false);
                connectingCanv.SetActive(false);
                events.SetActive(false);
                Instantiate(Resources.Load("GameManager"), new Vector3(0,0,0), new Quaternion(0,0,0,0));
                isActive = false;
                break;
            case kickCode:
                //Join request was rejected, reset attempt
                Debug.Log("Rejected");
                canv.SetActive(true);
                connectingCanv.SetActive(false);
                //Add message here, which classes are free? Is room full?
                break;
        }
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
        // we don't want to do anything if we are not attempting to join a room.
        // this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
        // we don't want to do anything.
        if (isConnecting)
        {
            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Random Join Failed, waiting for room");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        //PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        Connect();
    }

    public override void OnJoinedRoom()
    {
        //Irrelevant as we now use the accepted network event to handle having joined a room
    }

    #endregion

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// </summary>
    void Awake()
    {
        //Hide connecting screen
        connectingCanv.SetActive(false);
    }


    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// </summary>
    void Start()
    {
        
    }

    private void Update()
    {
        
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
        //Show player that connection attempt is being made
        connectingCanv.SetActive(true);
        canv.SetActive(false);
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

    //Called by canvas buttons, sets player nickname and attempts to join
    public void ClassSelected(string job)
    {
        PhotonNetwork.NickName = job;
        Connect();
    }


}
