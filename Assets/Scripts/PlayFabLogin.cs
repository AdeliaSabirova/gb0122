using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class PlayFabLogin : MonoBehaviour
{
    [SerializeField] private string _customId;
    [SerializeField] private bool _createAccount = true;

    [SerializeField] private TMP_Text _playFabStatus;

    private bool _loginStatus;

    private void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = " 9F2CE";
            Debug.Log("Title ID was installed");
        }
    }

    public void LoginPlayFab()
    {
        var request = new LoginWithCustomIDRequest { CustomId = _customId, CreateAccount = _createAccount };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    private void ChangeUIText(string label, Color color)
    {
        _playFabStatus.text = label;
        _playFabStatus.color = color;
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("PlayFab Success");
        ChangeUIText("Success", Color.green);
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError($"Fail: {error}");
        ChangeUIText("Failure", Color.red);
    }

}
