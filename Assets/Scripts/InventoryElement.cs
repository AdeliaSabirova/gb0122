using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class InventoryElement : MonoBehaviour
{
    [SerializeField] private TMP_Text _itemName;

    public void SetItem(ItemInstance item)
    {
        _itemName.text = item.DisplayName;
    }
}
