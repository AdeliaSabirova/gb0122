using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private CharactersElement _character;

    private const string _characterStoreId = "character_store";
    private const string _virtalCurrencyKey = "GC";

    private string _inputNameText;
    private string _inputExpText;
    private string _inputDamageText;
    private string _inputHealthText;



    void Start()
    {
        PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(),
            result =>
            {
                Debug.Log($"Characters count {result.Characters.Count}");
                UpdateCharacters();
            }, Debug.LogError);
        
    }

    public void OnNameChanged(string changedName)
    {
        _inputNameText = changedName;
    }

    public void OnExpChanged(string changedExp)
    {
        _inputExpText = changedExp;
    }

    public void OnDamageChanged(string changedDamage)
    {
        _inputDamageText = changedDamage;
    }
    public void OnHealthChanged(string changedHealth)
    {
        _inputHealthText = changedHealth;
    }

    public void OnCreateButtonCLicked()
    {
        if (string.IsNullOrEmpty(_inputNameText))
        {
            Debug.LogError("Input field should not be empty");
            return;
        }
        PlayFabClientAPI.GetStoreItems(new GetStoreItemsRequest
        {
            StoreId = _characterStoreId
        }, result => {
            HandleStoreResult(result.Store);
        }, Debug.LogError);
    }

    private void HandleStoreResult(List<StoreItem> items)
    {
        foreach(var item in items)
        {
            Debug.Log(item.ItemId);
            PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
            {
                ItemId = item.ItemId,
                VirtualCurrency = _virtalCurrencyKey,
                Price = (int)item.VirtualCurrencyPrices[_virtalCurrencyKey]
            },
            result =>
            {
                Debug.Log($"Item {result.Items[0].ItemId} was purchased");
                TransformItemIntoCharacter(result.Items[0].ItemId);
            }, Debug.LogError); ;
        }
    }

    private void TransformItemIntoCharacter(string itemId)
    {
        PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest
        {
            ItemId = itemId,
            CharacterName = _inputNameText
        }, result =>
        {
            UpdateCharacterStatistics(result.CharacterId);
            
        }, Debug.LogError);
    }

    private void UpdateCharacterStatistics(string characterId)
    {
        PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest
        {
            CharacterId = characterId,
            CharacterStatistics = new Dictionary<string, int>
            {
                {"HealthPoints", int.Parse(_inputHealthText)},
                { "Exp", int.Parse(_inputExpText)},
                { "Damage", int.Parse(_inputDamageText)}
            }
        }, result =>
        {
            Debug.Log("statistics updated");
            UpdateCharacters();
        }, Debug.LogError);
    }

    private void UpdateCharacters()
    {
        PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(),
            result =>
            {
                foreach(var character in result.Characters)
                {
                    var newCharacter = Instantiate(_character, _character.transform.parent);
                    
                    PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest
                    {
                        CharacterId = character.CharacterId
                    }, resultCallback =>
                    {
                        _character.SetItemName(character.CharacterName, resultCallback.CharacterStatistics["Exp"].ToString(), resultCallback.CharacterStatistics["Damage"].ToString(), resultCallback.CharacterStatistics["HealthPoints"].ToString());

                    }, Debug.LogError);

                    _character.gameObject.SetActive(true);
                }
            }, Debug.LogError);
    }
}
