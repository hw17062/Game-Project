using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class RPCCalls : MonoBehaviourPun
{
    [PunRPC]
    void test(string henlo)
    {
        Debug.Log(henlo);
    }
}
