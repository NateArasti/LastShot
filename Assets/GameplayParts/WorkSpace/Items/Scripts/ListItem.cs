using UnityEngine;
using UnityEngine.UI;

public class ListItem : MonoBehaviour
{
    [SerializeField] private Ingredient _overridenIngredient;
    [SerializeField] protected Image _icon;
    [SerializeField] protected Image _dragItemIcon;

    public Ingredient Item { get; private set; }

    public virtual void Start()
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