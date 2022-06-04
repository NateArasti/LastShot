using UnityEngine;

[System.Serializable]
public struct ContentStorage
{
    [SerializeField] private ItemContent.ItemType _type;
    [SerializeField] private RectTransform _contentPanel;
    [SerializeField] private string _typeName;

    public ItemContent.ItemType Type => _type;
    public RectTransform ContentPanel => _contentPanel;
    public string TypeName => _typeName;
}