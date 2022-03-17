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

    [SerializeField] private GameObject _itemImagePrefab;
    [SerializeField] private ContentStorage[] _contents;
    [SerializeField] private TMP_Text _typeNamePanel;
    [SerializeField] private ScrollRect _scrollRect;

    private ItemType _currentContentType;

    private void Start()
    {
        ShowCurrentContent();
    }

    public void FillContent(ItemType type, IReadOnlyCollection<IWorkItem> items)
    {
        var chosenContentPanel = _contents[(int)type].ContentPanel;
        foreach (var item in items)
        {
            Instantiate(_itemImagePrefab, chosenContentPanel.transform)
                .GetComponent<ListItem>().SetItem(item);
        }
    }

    public void SwitchRight()
    {
        _currentContentType = _currentContentType + 1;
        if ((int) _currentContentType > _contents.Length - 1)
            _currentContentType = 0;
        ShowCurrentContent();
    }

    public void SwitchLeft()
    {
        _currentContentType = _currentContentType - 1;
        if (_currentContentType < 0)
            _currentContentType = (ItemType) _contents.Length - 1;
        ShowCurrentContent();
    }

    private void ShowCurrentContent()
    {
        foreach (var content in _contents)
        {
            content.ContentPanel.SetActive(false);
        }

        _typeNamePanel.text = _contents[(int)_currentContentType].TypeName;
        _contents[(int)_currentContentType].ContentPanel.SetActive(true);
        _scrollRect.content = _contents[(int)_currentContentType].ContentPanel.GetComponent<RectTransform>();
    }
}
