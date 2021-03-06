using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(
    typeof(Image), 
    typeof(Button),
    typeof(BarShelfTooltipTrigger))]
public class ShelfIngredient : MonoBehaviour
{
    private Ingredient _ingredient;
    private Image _image;
    private Button _button;
    private BarShelfTooltipTrigger _tooltipTrigger;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
        _tooltipTrigger = GetComponent<BarShelfTooltipTrigger>();
        gameObject.SetActive(false);
    }

    public void SetIngredient(Ingredient ingredient, UnityAction<Ingredient, GameObject> chooseAction)
    {
        _ingredient = ingredient;
        _tooltipTrigger.Name = ingredient.Data.Name;
        _image.sprite = ingredient.Data.ObjectSprite;
        _image.SetNativeSize();
        _button.onClick.AddListener(() => chooseAction.Invoke(_ingredient, gameObject));
    }
}
