using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ShelfChosenIngredient : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    private Ingredient _ingredient;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void SetIngredient(Ingredient ingredient, UnityAction<Ingredient> unChooseAction)
    {
        _ingredient = ingredient;
        _iconImage.sprite = ingredient.Data.ObjectSprite;
        _button.onClick.AddListener(() => unChooseAction.Invoke(_ingredient));
        _button.onClick.AddListener(() => Destroy(gameObject));
    }
}