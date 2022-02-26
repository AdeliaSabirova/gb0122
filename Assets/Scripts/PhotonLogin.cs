using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhotonLogin : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _roomListPanel;
    [SerializeField] private GameObject _joinRandomRoomPanel;
    [SerializeField] private GameObject _createRoomPanel;
    [SerializeField] private GameObject _credentialsPanel;
    [SerializeField] private GameObject _storePanel;

    [SerializeField] private TMP_InputField _roomName;
    [SerializeField] private TMP_InputField _maxCount;
    [SerializeField] private Toggle _isRoomVisible;

    [SerializeField] private PlayersElement _player;
    [SerializeField] private RoomsElement _room;

    private Dictionary<string, RoomInfo> _cachedRoomList;
    private Dictionary<string, GameObject> _roomListEntries;

    private Dictionary<int, GameObject> _playerListEntries;

    #region UNITY

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        _cachedRoomList = new Dictionary<string, RoomInfo>();
        _roomListEntries = new Dictionary<string, GameObject>();
        _playerListEntries = new Dictionary<int, GameObject>();
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    #endregion

    #region PUN CALLBACKS

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Photon successfully connected");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Error on room creation: {returnCode}, {message}");
        CloseCreateRoomPanel();
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        _createRoomPanel.SetActive(false);
        Debug.Log("Room is created");
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        _cachedRoomList.Clear();
        ClearRoomListView();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        ClearRoomListView();
        UpdateCachedRoomList(roomList);
        UpdateRoomListView();
    }

    
    public override void OnLeftLobby()
    {
        base.OnLeftLobby();

        _cachedRoomList.Clear();
        ClearRoomListView();
    }


    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        _mainPanel.SetActive(false);
        _credentialsPanel.SetActive(false);
        _storePanel.SetActive(false);
        _joinRandomRoomPanel.SetActive(true);

        UpdatePlayerListView();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);

        //string roomName = "Room " + Random.Range(1000, 10000);
        //RoomOptions options = new RoomOptions { MaxPlayers = 8 };
        //PhotonNetwork.CreateRoom(roomName, options, null);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        ClearPlayerListView();

        _mainPanel.SetActive(true);
        _joinRandomRoomPanel.SetActive(false);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        _cachedRoomList.Clear();
        _roomListEntries.Clear();
        _playerListEntries.Clear();

        Debug.Log("Photon successfully disconnected");
    }

    #endregion

    public void OpenCreateRoomPanel()
    {
        _mainPanel.SetActive(false);
        _createRoomPanel.SetActive(true);
    }

    public void CloseCreateRoomPanel()
    {
        _mainPanel.SetActive(true);
        _createRoomPanel.SetActive(false);
    }

    public void OnCreateRoomButtonClicked()
    {
        string roomName = _roomName.text;
        roomName = (roomName.Equals(string.Empty)) ? "Room " + Random.Range(1000, 10000) : roomName;

        byte maxPlayers;
        byte.TryParse(_maxCount.text, out maxPlayers);
        maxPlayers = (byte)Mathf.Clamp(maxPlayers, 2, 8);

        bool isVisible = _isRoomVisible.isOn;

        if (!isVisible)
        {
            GUIUtility.systemCopyBuffer = roomName;
        }

        RoomOptions options = new RoomOptions { MaxPlayers = maxPlayers, PlayerTtl = 10000, IsVisible =  isVisible};

        PhotonNetwork.CreateRoom(roomName, options, null);
    }

    public void OnJoinRandomRoomButtonClicked()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public void OnLeaveRoomButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnLeaveLobbyButtonClicked()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

        _mainPanel.SetActive(true);
        _roomListPanel.SetActive(false);
    }


    public void OnRoomListButtonClicked()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
        _mainPanel.SetActive(false);
        _roomListPanel.SetActive(true);
    }

    public void OnStartGameButtonClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        PhotonNetwork.LoadLevel("ExampleScene");
    }


    private void UpdatePlayerListView()
    {
        foreach (var p in PhotonNetwork.PlayerList)
        {
            var newPlayer = Instantiate(_player, _player.transform.parent);
            newPlayer.gameObject.SetActive(true);
            newPlayer.SetItem(p);

            _playerListEntries.Add(p.ActorNumber, newPlayer.gameObject);
        }
    }

    private void ClearPlayerListView()
    {
        foreach (GameObject entry in _playerListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        _playerListEntries.Clear();
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
            {
                if (_cachedRoomList.ContainsKey(info.Name))
                {
                    _cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            if (_cachedRoomList.ContainsKey(info.Name))
            {
                _cachedRoomList[info.Name] = info;
            }

            else
            {
                _cachedRoomList.Add(info.Name, info);
            }
        }
    }


    private void UpdateRoomListView()
    {
        foreach (RoomInfo info in _cachedRoomList.Values)
        {
            var entry = Instantiate(_room, _room.transform.parent);
            entry.gameObject.SetActive(true);
            entry.SetItem(info);

            _roomListEntries.Add(info.Name, entry.gameObject);
        }
    }

    private void ClearRoomListView()
    {
        foreach (GameObject entry in _roomListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        _roomListEntries.Clear();
    }

}
