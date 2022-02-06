using PlayFab;
using PlayFab.ClientModels;
using System;
using TMPro;
using UnityEngine;

public class CatalogElement : MonoBehaviour
{
    [SerializeField] private TMP_Text _itemName;
    [SerializeField] private TMP_Text _price;
    [SerializeField] private TMP_Text _total;

    private StoreItem _item;

    private void Awake()
    {
        GetInventory();
    }
    private void GetInventory()
    {
        PlayFabClientAPI.GetUserInventory(
            new GetUserInventoryRequest { },
            result => {
                result.VirtualCurrency.TryGetValue("GC", out var value);
                _total.text = $"{value}";
            },
            Debug.LogError);
    }

    public void SetItem(StoreItem item)
    {
        _itemName.text = item.ItemId;
        _price.text = item.VirtualCurrencyPrices["GC"].ToString();
        _item = item;
    }

    public void BuyItem()
    {
        if (int.TryParse(_total.text, out var total) && int.TryParse(_price.text, out var price)) {
            if (price <= total)
            {
                Debug.Log("Success");
                _total.text = $"{total - price}";
            }
            else
                Debug.Log("Fail");
        }
        else
            Debug.Log("Error in parsing total or price value");
    }

    public void MakePurchase()
    {
        PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
        {
            ItemId = _item.ItemId,
            Price = (int)_item.VirtualCurrencyPrices["GC"],
            VirtualCurrency = "GC"
        }, result => { }, Debug.LogError);

        GetInventory();
    }
}
