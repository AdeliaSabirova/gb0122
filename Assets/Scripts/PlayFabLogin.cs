using PlayFab;
using PlayFab.ClientModels;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayFabLogin : MonoBehaviour
{
    [SerializeField] private TMP_Text _loadingMessage;

    private string _username;
    private string _mail;
    private string _pass;

    private const string AuthKey = "player-unique-id-auth-key";
    private const string AuthUsername = "player-username-auth-key";
    private const string AuthPassword = "player-pass-auth-key";

    private bool _loadingIndication = false;
    private bool _loadingAnimation = false;

    public void UpdateUsername(string username)
    {
        _username = username;
    }

    public void UpdateEmail(string mail)
    {
        _mail = mail;
    }

    public void UpdatePassword(string pass)
    {
        _pass = pass;
    }

    public void CreateAccount()
    {
        _loadingIndication = true;
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
        {
            Username = _username,
            Email = _mail,
            Password = _pass,
            RequireBothUsernameAndEmail = true
        }, result =>
        {
            Debug.Log($"Success: {_username}");
            RememberCredentials(_username, _pass);
            _loadingIndication = false;
            SceneManager.LoadScene("MainProfile");
        }, OnLoginFailure);
    }

    public void Login()
    {
        _loadingIndication = true;
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            Username = _username,
            Password = _pass
        }, result =>
        {
            RememberCredentials(_username, _pass);
            _loadingIndication = false;
            SceneManager.LoadScene("MainProfile");
        }, OnLoginFailure);
    }

    public void AnonymousLogin()
    {
        var needCreation = !PlayerPrefs.HasKey(AuthKey);
        Debug.Log($"needCreation = {needCreation}");
        var id = PlayerPrefs.GetString(AuthKey, Guid.NewGuid().ToString());
        Debug.Log($"id = {id}");
        var request = new LoginWithCustomIDRequest { CustomId = id, CreateAccount = needCreation };
        _loadingIndication = true;
        PlayFabClientAPI.LoginWithCustomID(request, result =>
        {
            PlayerPrefs.SetString(AuthKey, id);
            _loadingIndication = false;
            SceneManager.LoadScene("MainProfile");
        },
        OnLoginFailure);
    }

    private void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = " 9F2CE";
            Debug.Log("Title ID was installed");
        }

        if (PlayerPrefs.HasKey(AuthUsername) && PlayerPrefs.HasKey(AuthPassword))
        {
            _loadingIndication = true;
            PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
            {
                Username = PlayerPrefs.GetString(AuthUsername),
                Password = PlayerPrefs.GetString(AuthPassword)
            }, result =>
            {
                Debug.Log($"Login Success: {result.PlayFabId}");
                _loadingIndication = false;
                SceneManager.LoadScene("MainProfile");
            }, OnLoginFailure);
        }

        if (PlayerPrefs.HasKey(AuthKey))
        {
            _loadingIndication = true;
            PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest { 
                CustomId = PlayerPrefs.GetString(AuthKey), 
                CreateAccount = !PlayerPrefs.HasKey(AuthKey)
            }, result =>
            {
                _loadingIndication = false;
                SceneManager.LoadScene("MainProfile");
            },
        OnLoginFailure);
        }
    }
    private void LoadingIndication()
    {
        if (_loadingAnimation)
        {
            _loadingMessage.text = "....";
            _loadingAnimation = false;
        }
        else
        {
            _loadingMessage.text = "...";
            _loadingAnimation = true;
        }
    }

    private void Update()
    {
        if (_loadingIndication)
            LoadingIndication();
    }


    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError($"Fail: {error}");
        _loadingIndication = false;
        _loadingMessage.text = $"Error... {error}";
    }

    private void RememberCredentials(string username, string pass)
    {
        PlayerPrefs.SetString(AuthUsername, username);
        PlayerPrefs.SetString(AuthPassword, pass);
    }
}
