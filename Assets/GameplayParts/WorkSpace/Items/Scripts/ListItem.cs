using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
class ListItem : MonoBehaviour
{
    [SerializeField] private Sprite _simpleSprite;
    [SerializeField] private Sprite _chosenSprite;
    [Space(20f)]
    [SerializeField] private Image _icon;
    [SerializeField] private Image _dragItemIcon;

    public Ingredient Item { get; private set; }


    public void SetItem(Ingredient item)
    {
        Item = item;
        _icon.sprite = item.Sprite;
        _dragItemIcon.sprite = item.Sprite;
    }
}