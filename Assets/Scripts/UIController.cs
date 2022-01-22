
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Button _playFabConnect;
    [SerializeField] private Button _photonConnect;
    [SerializeField] private Button _photonDisconnect;

    [SerializeField] private PlayFabLogin _playFabLogin;
    [SerializeField] private PhotonLogin _photonLogin;

    private void Start()
    {
        _playFabConnect.onClick.AddListener(ConnectToPlayFab);
        _photonConnect.onClick.AddListener(ConnectToPhoton);
        _photonDisconnect.onClick.AddListener(DisconnectFromPhoton);
    }

    private void DisconnectFromPhoton()
    {
        _photonLogin.Disconnect();
    }

    private void ConnectToPhoton()
    {
        _photonLogin.Connect();
    }

    private void ConnectToPlayFab()
    {
        _playFabLogin.LoginPlayFab();
    }
}
