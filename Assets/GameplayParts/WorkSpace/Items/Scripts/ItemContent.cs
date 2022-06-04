using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemContent : MonoBehaviour
{
    public enum ItemType
    {
        Alcohol,
        Ingredient,
        Instrument
    }

    [SerializeField] private ListItem _itemImagePrefab;
    [SerializeField] private ContentStorage[] _contents;
    [SerializeField] private ScrollRect _itemsScrollRect;
    [SerializeField] private TMP_Text _itemsTittle;
    private int _currentItemsType;

    public void FillContent(ItemType type, IEnumerable<Ingredient> items)
    {
        var chosenContentPanel = _contents[(int)type].ContentPanel;
        foreach (var item in items)
        {
            Instantiate(_itemImagePrefab, chosenContentPanel.transform).SetItem(item);
        }
    }

    public void SwitchPage(int switchAmount)
    {
        _contents[_currentItemsType].ContentPanel.gameObject.SetActive(false);
        _currentItemsType = (_currentItemsType + switchAmount) % 3;
        _itemsScrollRect.content = _contents[_currentItemsType].ContentPanel;
        _itemsTittle.text = _contents[_currentItemsType].TypeName;
        _contents[_currentItemsType].ContentPanel.gameObject.SetActive(true);
    }
}

