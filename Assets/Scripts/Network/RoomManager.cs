using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Unity.VisualScripting;
using Photon.Realtime;
using System;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class RoomManager : PersistentPhotonSingleton<RoomManager>
{
    public string currentRoomName;
    public List<RoomInfo> roomList;
    public List<Player> playerList;
    private readonly TypedLobby sqlLobby = new TypedLobby("SQLLOBBY", LobbyType.SqlLobby);
    public const string ELO_PROP_KEY = "elo";
    public const string MAP_PROP_KEY = "map";
    public const byte MAX_PLAYERS = 2;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (String.IsNullOrEmpty(currentRoomName) && PhotonNetwork.InRoom)
        {
            currentRoomName = PhotonNetwork.CurrentRoom.Name;
        }
    }

    public void CreateRoom(string roomName, Hashtable roomProperties)
    {
        if (PhotonNetwork.OfflineMode)
        {
            Debug.Log("Player is offline");
            return;
        }
        
        if (string.IsNullOrEmpty(roomName))
        {
            Debug.Log("Room name is empty");
            return;
        }

        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = MAX_PLAYERS
        };
        roomOptions.CustomRoomProperties = new HashTable{ { ELO_PROP_KEY, roomProperties["elo"] }, { MAP_PROP_KEY, roomProperties["map"]} };
        roomOptions.CustomRoomPropertiesForLobby = new string[] { ELO_PROP_KEY, MAP_PROP_KEY };

        PhotonNetwork.CreateRoom(roomName,  roomOptions);
    }

    public void JoinRoom(string roomName)
    {
        if (string.IsNullOrEmpty(roomName))
        {
            Debug.Log("Room name is empty");
            return;
        }

        PhotonNetwork.JoinRoom(roomName);
    }

    public void LeaveRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            Debug.Log("Not in room");
        }
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("Created room");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("Create room failed: " + message);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined room");
        PhotonNetwork.LoadLevel("");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("Join room failed: " + message);
    }

    public void GetRoomList(string sqlFilter = "") {
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("Already in room");
            return;
        }

        PhotonNetwork.GetCustomRoomList(TypedLobby.Default, sqlFilter);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        base.OnRoomListUpdate(roomList);

        if (roomList.Count > 0)
        {
            this.roomList.Clear();
        }

        foreach (RoomInfo roomInfo in roomList)
        {
            this.roomList.Add(roomInfo);
        }
        Debug.Log("Room list updated");
    }
}