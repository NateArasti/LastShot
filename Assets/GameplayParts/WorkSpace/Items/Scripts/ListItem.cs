using UnityEngine;
using UnityEngine.UI;

class ListItem : MonoBehaviour
{
    [SerializeField] private Ingredient _overridenIngredient;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _dragItemIcon;

    public Ingredient Item { get; private set; }

    private void Start()
    {
        if (_overridenIngredient == null) return;
        SetItem(_overridenIngredient);
    }

    public void SetItem(Ingredient item)
    {
        Item = item;
        _icon.sprite = item.Icon;
        _dragItemIcon.sprite = item.WorkSprite;
    }
}