using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby;

    public GameObject battleButton;
    public GameObject cancelButton;

    private void Awake()
    {
        lobby = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("YO DUDE YOU JUST CONNECTED TO PHOTON");
        PhotonNetwork.AutomaticallySyncScene = true;
        //  battleButton.SetActive(true);
    }

    public void OnBattleButtonClicked()
    {
        cancelButton.SetActive(true);
        battleButton.SetActive(false);
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("DAT ROOM JOIN FAILED");
        CreateRoom();
    }

    void CreateRoom()
    {
        Debug.Log("pls sir dont whip me im trying to create a room");
        int randomRoomName = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSetting.multiplayerSetting.maxPlayers };
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create room BUT IT DIDN'T WORK BECAUSE YOUR ROOM NAME IS TO GENERIC");
        CreateRoom();
    }

    public void OnCancelButtonClicked()
    {
        Debug.Log("WHY YOU PRESS CANCEL");
        cancelButton.SetActive(false);
        battleButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
}
