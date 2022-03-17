using UnityEngine;

[System.Serializable]
public struct ContentStorage
{
    [SerializeField] private ItemContent.ItemType _type;
    [SerializeField] private GameObject _contentPanel;
    [SerializeField] private string _typeName;

    public ItemContent.ItemType Type => _type;
    public GameObject ContentPanel => _contentPanel;
    public string TypeName => _typeName;
}
