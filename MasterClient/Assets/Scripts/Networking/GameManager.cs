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

    //[SerializeField]
    //GameObject classPrefab;

    #endregion

    #region Private Fields

    private bool alreadyScanning = false;

    private GameObject cam;

    private GameObject canv;

    private ScanCards scan;

    private string myName = "MasterClient";

    private int validPlayerCount = 0;

    private Dictionary<string, ClientStat> players = new Dictionary<string, ClientStat>() { //Could store player IDs instead
        {"Warrior", new ClientStat()},
        {"Ranger", new ClientStat()},
        {"Mage", new ClientStat()},
        {"Cleric", new ClientStat()},
        {"DungeonMaster", new ClientStat()},
        {"MasterClient", new ClientStat()}
    };

    private Queue<string> turnOrder = new Queue<string>();

    #endregion

    #region Event Codes
    //THIS REGION NEEDS TO BE THE SAME ACROSS ALL SCRIPTS THAT USE EVENT HANDLING
    //CURRENTLY: Launcher.cs, GameManager.cs in ARGRID and GameManager.cs in master client

    const byte kickCode = 1;
    const byte acceptPlayerCode = 2;
    const byte moveCode = 3;
    const byte msgCode = 4;

    #endregion

    //#region RPC Methods

    //[PunRPC]
    //void test(string henlo)
    //{
    //    Debug.Log(henlo);
    //}

    //#endregion

    #region Public Fields

    //[Tooltip("The prefab to use for representing the player")]
    //public GameObject warrior;
    //public GameObject ranger;
    //public GameObject mage;
    //public GameObject cleric;
    //public GameObject dm;

    #endregion

    #region Photon Callbacks

    public override void OnPlayerEnteredRoom(Player other)
    {
        //Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


        if (PhotonNetwork.IsMasterClient)
        {
            //Debug.Log(other.NickName);
            ClientStat value;
            if (!players.TryGetValue(other.NickName, out value))
            {
                //Debug.Log("Out1");
                PhotonNetwork.CloseConnection(other); //Boot player for invalid name
                return;
            }
            //Debug.Log("1");
            if (value.getPres())
            {
                //Debug.Log("Out2");
                object[] content = new object[] { other.UserId, other.NickName };
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                SendOptions sendOptions = new SendOptions { Reliability = true };
                PhotonNetwork.RaiseEvent(kickCode, content, raiseEventOptions, sendOptions);
                PhotonNetwork.CloseConnection(other); //Boot player for using existing name
            }
            else
            {
                //Debug.Log("2");
                players[other.NickName] = new ClientStat(other.NickName);
                //Debug.Log("3");
                validPlayerCount++;
                if (!other.NickName.Equals(myName)) turnOrder.Enqueue(other.NickName);
                Debug.Log(other.NickName);

                object[] content = new object[] { }; // Array contains the target position and the IDs of the selected units
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
                SendOptions sendOptions = new SendOptions { Reliability = true };
                PhotonNetwork.RaiseEvent(acceptPlayerCode, content, raiseEventOptions, sendOptions);
                //GameObject temp;
                //temp = PhotonNetwork.Instantiate(warrior.name, new Vector3(0,0,0), new Quaternion(0,0,0,0));
                //players[other.NickName].setRpc(temp);
            }
        }

    }


    public override void OnPlayerLeftRoom(Player other)
    {
        //Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


        ClientStat value;
        if (players.TryGetValue(other.NickName, out value)) //Sanity check, to make sure we won't crash part way through
        {
            players[other.NickName].setPres(false);
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
        cam = GameObject.Find("Main Camera");
        canv = GameObject.Find("Canvas");
        scan = cam.GetComponent<ScanCards>();
    }

    private void Start()
    {
        //StartCoroutine(ExampleCoroutine());
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
        //string currentPlayer = turnOrder.Peek();
        //if (players[currentPlayer].getAP() == 0) //If current player out of AP move them to back of queue
        //{
        //    turnOrder.Dequeue();
        //    turnOrder.Enqueue(currentPlayer);
        //}
        //Debug.Log(turnOrder.Count);
        //Debug.Log(scan.CamReady);
        if ((!alreadyScanning) && scan.CamReady && (turnOrder.Count > 0))
        {

            scan.StartScan(this);
            alreadyScanning = true;
        }
        //this.photonView.RPC("test", RpcTarget.All, "Hot Diggity Damn");
        //Debug.Log(card.cardname);
        //if (card == null) return; //Invalid card or Scanner isn't working
        //Card logic, validate cards
        //if (!card.type.Equals(currentPlayer) && !card.type.Equals("basic")) return; //Wrong card for current player
        //if (players[currentPlayer].getAP() < card.ap)
        //{
        //    //Maybe add RPC call to inform player
        //    return; //Not enough AP for this card
        //}
        ////RPC call, sending card to players
        //players[currentPlayer].setAP(players[currentPlayer].getAP() - card.ap); //Adjust AP for card just used
    }

    private void CardHandle(Card card)
    {
        //string currentPlayer = turnOrder.Peek();
        //if (players[currentPlayer].getAP() == 0) //If current player out of AP move them to back of queue
        //{
        //    turnOrder.Dequeue();
        //    turnOrder.Enqueue(currentPlayer);
        //    alreadyScanning = false;
        //    return;
        //}
        Debug.Log(card.cardname);
        if (card == null)
        {
            alreadyScanning = false;
            return; //Invalid card or Scanner isn't working
        }
        ////Card logic, validate cards
        //if (!card.type.Equals(currentPlayer) && !card.type.Equals("basic")) return; //Wrong card for current player
        //if (players[currentPlayer].getAP() < card.ap)
        //{
        //    //Maybe add RPC call to inform player
        //    return; //Not enough AP for this card
        //}
    }

    #endregion

    #region Public Methods

    public void scannerCallback(Card card)
    {
        CardHandle(card);
    }


    public void LeaveRoom()
    {
        //RPC to force kick all players when masterClient leaves
        PhotonNetwork.LeaveRoom();
    }


    #endregion
}

