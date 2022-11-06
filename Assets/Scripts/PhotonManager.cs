using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PhotonManager : MonoBehaviourPunCallbacks {
    private readonly string version = "1.0";
    private string userId = "Araina";

    public TMP_InputField userIF;
    public TMP_InputField roomNameIF;

    private Dictionary<string, GameObject> rooms = new Dictionary<string, GameObject>();
    private GameObject roomItemPrefab;
    public Transform scrollContent;


    private void Awake() {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = version;
        //PhotonNetwork.NickName = userId;
        
        Debug.Log(PhotonNetwork.SendRate);  // Data Send Per Sec.

        roomItemPrefab = Resources.Load<GameObject>("RoomItem");

        if (PhotonNetwork.IsConnected == false) {
            PhotonNetwork.ConnectUsingSettings();   // Server Connect
        }
    }

    private void Start() {
        userId = PlayerPrefs.GetString("USER_ID", $"USER_{Random.Range(1, 21):00}");
        userIF.text = userId;
        PhotonNetwork.NickName = userId;
    }

    public void SetUserId() {
        if (string.IsNullOrEmpty(userIF.text)) {
            userId = $"User_{Random.Range(1, 21):00}";
        }
        else {
            userId = userIF.text;
        }

        PlayerPrefs.SetString("USER_ID", userId);
        PhotonNetwork.NickName = userId;
    }

    public string SetRoomName() {
        if (string.IsNullOrEmpty(roomNameIF.text)) {
            roomNameIF.text = $"ROOM_{Random.Range(1, 101):000}";
        }
        
        return roomNameIF.text;
    }

    public override void OnConnectedToMaster() {
        Debug.Log("Connected to Master!");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby() {
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        //PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        Debug.Log($"JoinRandom Failed {returnCode}:{message}");

        OnMakeRoomClick();

        // RoomOptions ro = new RoomOptions();
        // ro.MaxPlayers = 20;
        // ro.IsOpen = true;
        // ro.IsVisible = true;

        // PhotonNetwork.CreateRoom("My Room", ro);
    }

    public override void OnCreatedRoom() {
        Debug.Log("Created Room");
        Debug.Log($"Room name = {PhotonNetwork.CurrentRoom.Name}");
    }

    public override void OnJoinedRoom() {
        Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}");
        Debug.Log($"Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");
        
        foreach (var player in PhotonNetwork.CurrentRoom.Players) {
            Debug.Log($"{player.Value.NickName}, {player.Value.ActorNumber}");
        }

        if (PhotonNetwork.IsMasterClient) {
            PhotonNetwork.LoadLevel("BattleField");
        }

        // Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        // int idx = Random.Range(1, points.Length);

        // PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation, 0);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        GameObject tempRoom = null;
        
        foreach (var room in roomList) {
            if (room.RemovedFromList == true) {
                rooms.TryGetValue(room.Name, out tempRoom);
                Destroy(tempRoom);
                rooms.Remove(room.Name);
            }
            else {
                if (rooms.ContainsKey(room.Name) == false) {
                    GameObject roomPrefab = Instantiate(roomItemPrefab, scrollContent);
                    roomPrefab.GetComponent<RoomData>().RoomInfo = room;
                    rooms.Add(room.Name, roomPrefab);
                }
                else {
                    rooms.TryGetValue(room.Name, out tempRoom);
                    tempRoom.GetComponent<RoomData>().RoomInfo = room;
                }
            }
        }
    }

#region UI_BUTTON_EVENT
    public void OnLoginClick() {
        SetUserId();

        PhotonNetwork.JoinRandomRoom();
    }

    public void OnMakeRoomClick() {
        SetUserId();

        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 20;
        ro.IsOpen = true;
        ro.IsVisible = true;    

        PhotonNetwork.CreateRoom(SetRoomName(), ro);
    }
#endregion
}