using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatalogManager : MonoBehaviour
{
    [SerializeField] private CatalogElement _element;

    private readonly Dictionary<string, CatalogItem> _catalog = new Dictionary<string, CatalogItem>();

    private void Start()
    {
        PlayFabClientAPI.GetStoreItems(new GetStoreItemsRequest
        {
            StoreId = "store_1"
        }, result =>
        {
            HandleStore(result.Store);
        }, error =>
        {
            Debug.LogError($"Get Store Items Failed: {error}");
        }) ;
    }

    private void HandleStore(List<StoreItem> store)
    {
        foreach (var item in store)
        {
            var element = Instantiate(_element, _element.transform.parent);
            element.gameObject.SetActive(true);
            element.SetItem(item);
        }
    }
}
