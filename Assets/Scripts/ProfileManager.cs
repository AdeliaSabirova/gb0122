using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProfileManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _id;
    [SerializeField] private TMP_Text _username;
    [SerializeField] private TMP_Text _created;

    private bool _loadingIndication = false;
    private bool _loadingAnimation = false;

    void Start()
    {
        _loadingIndication = true;
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), success =>
        {
            _loadingIndication = false;
            _id.text = $"Welcome back, Player\nPlayFab Id: {success.AccountInfo.PlayFabId}";
            _username.text = $"Username: {success.AccountInfo.Username}";
            _created.text = $"Created at: {success.AccountInfo.Created}";

        }, error =>
        {
            _loadingIndication = false;
            _id.text = $"Error: {error}";
        });
    }

    public void DeleteSaved()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Bootstrap");
    }
    public void OpenGame()
    {
        SceneManager.LoadScene("ExampleScene");
    }

    private void LoadingIndication()
    {
        if (_loadingAnimation)
        {
            _id.text = "....";
            _loadingAnimation = false;
        }
        else
        {
            _id.text = "...";
            _loadingAnimation = true;
        }
    }

    private void Update()
    {
        if (_loadingIndication)
            LoadingIndication();
    }
}
