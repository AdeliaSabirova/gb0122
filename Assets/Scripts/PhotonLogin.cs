using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PhotonLogin : MonoBehaviourPunCallbacks
{
    //[SerializeField] private TMP_Text _photonStatus;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        Connect();
    }

    public void Connect()
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

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    private void ChangeUIText(string label, Color color)
    {
        //_photonStatus.text = label;
        //_photonStatus.color = color;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log("Photon successfully disconnected");
        ChangeUIText("Disconnected", Color.grey);
    }


    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Photon successfully connected");
        ChangeUIText("Connected", Color.green);
    }
}
