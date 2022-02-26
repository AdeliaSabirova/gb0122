using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomsElement : MonoBehaviour
{
    [SerializeField] private TMP_Text _itemName;
    [SerializeField] private TMP_Text _itemMax;
    [SerializeField] private TMP_Text _itemPlayers;
    [SerializeField] private Toggle _itemIsOpen;

    public void SetItem(RoomInfo item)
    {
        _itemName.text = $"RoomName: {item.Name}";
        _itemMax.text = $"MaxPlayers: {item.MaxPlayers}";
        _itemPlayers.text = $"Current: {item.PlayerCount}";
        _itemIsOpen.isOn = item.IsOpen;
    }
}
