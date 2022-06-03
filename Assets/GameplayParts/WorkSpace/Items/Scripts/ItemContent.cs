
using System.Collections.Generic;
using System.Web.UI.WebControls;
using UnityEngine;

public class ItemContent : MonoBehaviour
{
    public enum ItemType
    {
        Alcohol,
        Ingredient,
        Instrument
    }

    [SerializeField] private GameObject _itemImagePrefab;
    [SerializeField] private ContentStorage[] _contents;

    public void FillContent(ItemType type, IReadOnlyCollection<Ingredient> items)
    {
        var chosenContentPanel = _contents[(int)type].ContentPanel;
        foreach (var item in items)
        {
            Instantiate(_itemImagePrefab, chosenContentPanel.transform)
                    .GetComponent<ListItem>().SetItem(item);
        }
    }


}

