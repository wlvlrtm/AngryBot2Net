using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;   
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks {
    public TMP_Text roomName;
    public TMP_Text connectInfo;
    public TMP_Text msgList;
    public Button exitBtn;
    

    private void Awake() {
        CreatePlayer();
        SetRoomInfo();

        exitBtn.onClick.AddListener(() => OnExitClick());
    }

    private void CreatePlayer() {
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int indx = Random.Range(1, points.Length);

        PhotonNetwork.Instantiate("Player", points[indx].position, points[indx].rotation, 0);
    }

    public void SetRoomInfo() {
        Room room = PhotonNetwork.CurrentRoom;
        roomName.text = room.Name;
        connectInfo.text = $"({room.PlayerCount}/{room.MaxPlayers})";
    }

    private void OnExitClick() {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom() {
        SceneManager.LoadScene("Lobby");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        SetRoomInfo();
        string msg = $"\n<color=#00ff00>{newPlayer.NickName}</color> is joined room";
        msgList.text += msg;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        SetRoomInfo();
        string msg = $"\n<color=#ff0000>{otherPlayer.NickName}</color> is Left room";
        msgList.text += msg;
    }

}
