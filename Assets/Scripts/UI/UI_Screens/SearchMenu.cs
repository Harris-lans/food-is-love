﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchMenu : UIScreen
{

    [Space, Header("Screens to switch to")]
    [SerializeField]
    private SO_Tag _FoodWorldScreenTag;
    [SerializeField]
    private SO_Tag _WaitingMenuScreenTag;

    private PhotonNetworkManager _PhotonNetworkManager;
    private LobbyManager _LobbyManager;

    protected override void Start()
    {
        base.Start();

        _PhotonNetworkManager = PhotonNetworkManager.Instance;
        _LobbyManager = LobbyManager.Instance;

        // Telling the network manager to create a room as there are no rooms to join
        _PhotonNetworkManager.OnPlayerFailedToJoinRoomEvent.AddListener(OnJoinGameFailed);
        _PhotonNetworkManager.OnJoinedRoomEvent.AddListener(OnJoinedGame);
    }

    public void OnClickBackFoodWorld()
    {
        _UIManager.SetScreen(_FoodWorldScreenTag);
        _PhotonNetworkManager.LeaveGame();
    }

    private void OnJoinGameFailed()
    {
        Debug.Log("Failed to join game");
        _UIManager.SetScreen(_WaitingMenuScreenTag);
    }

    private void OnJoinedGame()
    {
        Debug.Log("Joined Game");
        _LobbyManager.ReadyUp();
    }
}
