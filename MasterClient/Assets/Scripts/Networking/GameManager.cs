using System;
using System.Collections;
using System.Collections.Generic;


using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class GameManager : MonoBehaviourPunCallbacks
{

    #region Private Serializable Fields

    #endregion

    #region Private Fields

    private bool alreadyScanning = false;

    private GameObject cam;

    private GameObject canv;

    private ScanCards scan;

    private string myName = "MasterClient";

    private int validPlayerCount = 0;

    //Dictionary to store currently active players
    private Dictionary<string, ClientStat> players = new Dictionary<string, ClientStat>() { //Could store player IDs instead
        {"Warrior", new ClientStat()},
        {"Ranger", new ClientStat()},
        {"Mage", new ClientStat()},
        {"Cleric", new ClientStat()},
        {"DungeonMaster", new ClientStat()},
        {"MasterClient", new ClientStat()} //Extra entry in case we needed to store out status for some reason
    };

    //Stores turn order, currently implemented as a queue as we order turns by first connection goes first
    private Queue<string> turnOrder = new Queue<string>();

    #endregion

    #region Event Codes
    //THIS REGION NEEDS TO BE THE SAME ACROSS ALL SCRIPTS THAT USE EVENT HANDLING
    //CURRENTLY: Launcher.cs, GameManager.cs in ARGRID and GameManager.cs in master client
    //Variables are const as they are sued in switch case statements, feel free to change to if else
    const byte kickCode = 1;
    const byte acceptPlayerCode = 2;
    const byte moveCode = 3;
    const byte msgCode = 4;

    #endregion

    #region Public Fields

    #endregion

    #region Photon Callbacks

    public override void OnPlayerEnteredRoom(Player other)
    {

        //Sanity check that tthis client initiated the server
        if (PhotonNetwork.IsMasterClient)
        {
            ClientStat value;
            if (!players.TryGetValue(other.NickName, out value)) //Test if player with connecting nickname is in list of valid players
            {
                PhotonNetwork.CloseConnection(other); //Boot player for invalid name
                return;
            }
            if (value.getPres()) //Test if the name is already taken
            {
                //This code raises a network event that will boot the player
                object[] content = new object[] { other.UserId, other.NickName };
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                SendOptions sendOptions = new SendOptions { Reliability = true };
                PhotonNetwork.RaiseEvent(kickCode, content, raiseEventOptions, sendOptions);
                PhotonNetwork.CloseConnection(other); //Boot player for using existing name
            }
            else
            {
                //Update fact that class has been taken
                players[other.NickName] = new ClientStat(other.NickName);
                validPlayerCount++;
                if (!other.NickName.Equals(myName)) turnOrder.Enqueue(other.NickName);
                Debug.Log(other.NickName);

                //Raise a network event that accepts the player
                object[] content = new object[] { }; // Array contains the target position and the IDs of the selected units
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
                SendOptions sendOptions = new SendOptions { Reliability = true };
                PhotonNetwork.RaiseEvent(acceptPlayerCode, content, raiseEventOptions, sendOptions);
            }
        }
        //Maybe add code to leave room if we aren't original master client?
    }


    public override void OnPlayerLeftRoom(Player other)
    {

        ClientStat value;
        if (players.TryGetValue(other.NickName, out value)) //Sanity check, to make sure we won't crash part way through
        {
            //Free up class taken by leaving player
            players[other.NickName].setPres(false);
            validPlayerCount--;
        }
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
        cam = GameObject.Find("Main Camera");
        canv = GameObject.Find("Canvas");
        scan = cam.GetComponent<ScanCards>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if ((!alreadyScanning) && scan.CamReady && (turnOrder.Count > 0))
        {

            scan.StartScan(this);
            alreadyScanning = true;
        }
    }

    //Function for logic to handle scanned cards
    private void CardHandle(Card card)
    {
        Debug.Log(card.cardname);
        if (card == null)
        {
            alreadyScanning = false;
            return; //Invalid card or Scanner isn't working
        }
    }

    #endregion

    #region Public Methods

    public void scannerCallback(Card card)
    {
        CardHandle(card);
    }


    public void LeaveRoom()
    {
        //Will want to raise an event that tells all players to leave room when
        //master client leaves
        PhotonNetwork.LeaveRoom();
    }


    #endregion
}

