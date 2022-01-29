using PlayFab.ClientModels;
using System;
using TMPro;
using UnityEngine;

public class CatalogElement : MonoBehaviour
{
    [SerializeField] private TMP_Text _itemName;
    [SerializeField] private TMP_Text _price;
    [SerializeField] private TMP_Text _total;

    public void SetItem(CatalogItem item)
    {
        _itemName.text = item.DisplayName;
        _price.text = item.VirtualCurrencyPrices["GC"].ToString();
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
}
