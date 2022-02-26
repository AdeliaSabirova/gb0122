using TMPro;
using UnityEngine;

public class CharactersElement : MonoBehaviour
{
    [SerializeField] private TMP_Text _itemName;
    [SerializeField] private TMP_Text _itemExp;
    [SerializeField] private TMP_Text _itemDamage;
    [SerializeField] private TMP_Text _itemHealth;

    public void SetItemName(string name, string exp, string damage, string health)
    {
        _itemName.text = $"Name: {name}";
        _itemExp.text = $"Experience: {exp}";
        _itemDamage.text = $"Damage Coeff: {damage}";
        _itemHealth.text = $"Health Points: {health}";
    }

}
