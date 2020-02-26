using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class GameManager : MonoBehaviourPunCallbacks
{

    #region Event Codes
    //THIS REGION NEEDS TO BE THE SAME ACROSS ALL SCRIPTS THAT USE EVENT HANDLING
    //CURRENTLY: Launcher.cs, GameManager.cs in ARGRID and GameManager.cs in master client

    const byte kickCode = 1;
    const byte acceptPlayerCode = 2;
    const byte moveCode = 3;
    const byte msgCode = 4;

    #endregion

    #region Private Fields

    private GameObject myObj;
    private Unit myUnit;

    #endregion

    #region Public Fields

    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;

    public bool instState = true;

    #endregion

    #region Photon Callbacks

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == 1)
        {
            Debug.Log("Hope");
        }

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
        base.OnEnable(); //DEFINITIELY NOT THIS ONE
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    public override void OnDisable()
    {
        base.OnDisable(); //OH NO YOU DON'T
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }


    public override void OnPlayerEnteredRoom(Player other)
    {
        //Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


        //if (PhotonNetwork.IsMasterClient)
        //{
        //    Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


        //    LoadArena();
        //}
    }


    public override void OnPlayerLeftRoom(Player other)
    {
        //Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


        //if (PhotonNetwork.IsMasterClient)
        //{
        //    Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


        //    LoadArena();
        //}
    }

    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        //SceneManager.LoadScene(0);
    }


    #endregion

    #region Private Methods

    private void Awake()
    {

    }

    private void Start()
    {
        if (!instState) return;
        //Debug.Log("Start");
        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            //Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            if (GridObserver.LocalPlayerInstance == null)
            {
                Debug.Log("Instantiating");
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                myObj = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                myUnit = myObj.GetComponent<Unit>();
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }
    }

    //void LoadArena()
    //{
    //    if (!PhotonNetwork.IsMasterClient)
    //    {
    //        Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
    //    }
    //    Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
    //    PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
    //}


    #endregion

    #region Public Methods


    public void LeaveRoom()
    {

        //PhotonNetwork.Destroy(photonView);
        PhotonNetwork.LeaveRoom();
        //Application.Quit();
    }


    #endregion
}
