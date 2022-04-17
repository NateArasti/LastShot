using UnityEngine;

[System.Serializable]
public class InfoData
{
    [SerializeField] protected Sprite _sprite;
    [SerializeField] protected string _name;
    [TextArea(5, 20)] [SerializeField] protected string _description;

    public Sprite ObjectSprite
    {
        get => _sprite;
        set => _sprite = value;
    }

    public string Name => _name;

    public string Description => _description;
}
