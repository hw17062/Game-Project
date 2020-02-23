using System;
using System.Collections;
using System.Collections.Generic;


using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;



public class GameManager : MonoBehaviourPunCallbacks
{

    #region Private Fields

    private String myName = "MasterClient";

    private int validPlayerCount = 0;

    private Dictionary<String, bool> players = new Dictionary<String, bool>() { //Could store player IDs instead
        {"Warrior", false},
        {"Ranger", false},
        {"Mage", false},
        {"Cleric", false},
        {"DungeonMaster", false},
        {"MasterClient", false}
    };

    //private Queue

    #endregion

    #region Public Fields

    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;

    #endregion

    #region Photon Callbacks


    public override void OnPlayerEnteredRoom(Player other)
    {
        //Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


        if (PhotonNetwork.IsMasterClient)
        {
            bool value;
            if (!players.TryGetValue(other.NickName, out value))
            {
                PhotonNetwork.CloseConnection(other); //Boot player for invalid name
                return;
            }
            if (value)
            {
                PhotonNetwork.CloseConnection(other); //Boot player for using existing name
            } else
            {
                players[other.NickName] = true;
                validPlayerCount++;
            }
        }

    }


    public override void OnPlayerLeftRoom(Player other)
    {
        //Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


        bool value;
        if (players.TryGetValue(other.NickName, out value)) //Sanity check, to make sure we won't crash part way through
        {
            players[other.NickName] = false;
            validPlayerCount--;
        }
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
        //players.Add("Warrior", false);
        //players.Add("Ranger", false);
        //players.Add("Bard", false);
        //players.Add("Cleric", false);
        //players.Add("DungeonMaster", false);
    }

    private void Start()
    {
        //if (playerPrefab == null)
        //{
        //    Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        //}
        //else
        //{
        //    //Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
        //    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
        //    if (GridObserver.LocalPlayerInstance == null)
        //    {
        //        Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
        //        // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
        //        PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(5, 1, 5), Quaternion.identity, 0);
        //    }
        //    else
        //    {
        //        Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
        //    }
        //}
    }

    private void Update()
    {
        
    }

    #endregion

    #region Public Methods


    public void LeaveRoom()
    {
        //RPC to force kick all players when masterClient leaves
        PhotonNetwork.LeaveRoom();
    }


    #endregion
}

