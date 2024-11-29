using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TurnManager : PhotonSingleton<TurnManager>, IPunObservable
{
    public int turnID = 1;

    void Start()
    {

    }

    void Update()
    {
        
    }

    [PunRPC]
    public void AdvanceTurn()
    {
        turnID++;
        Debug.Log(PhotonNetwork.PlayerList.Length + 1 + (PhotonNetwork.OfflineMode ? 1 : 0));
        
        if (turnID >= PhotonNetwork.PlayerList.Length + 1 + (PhotonNetwork.OfflineMode? 1: 0))
        {
            turnID = 1;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(turnID);
        }
        else
        {
            turnID = (int)stream.ReceiveNext();
        }
    }
}