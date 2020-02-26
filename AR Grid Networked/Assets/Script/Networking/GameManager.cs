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


    //Test event handling code
    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == 4)
        {
            Debug.Log("Hope");
        }

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

    }


    public override void OnPlayerLeftRoom(Player other)
    {

    }

    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {

    }


    #endregion

    #region Private Methods

    private void Awake()
    {

    }

    private void Start()
    {
        //Safety measure against duplicate GameManagers
        if (!instState) return;

        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
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


    #endregion

    #region Public Methods


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }


    #endregion
}
