using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PlayersElement : MonoBehaviour
{
    [SerializeField] private TMP_Text _itemName;

    public void SetItem(Player item)
    {
        _itemName.text = item.UserId;
    }
}
