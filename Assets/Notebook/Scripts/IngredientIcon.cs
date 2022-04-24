using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class IngredientIcon : MonoBehaviour
{
    [SerializeField] private Image _icon;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void SetData(Ingredient ingredient, UnityAction<Ingredient> chooseAction)
    {
        _icon.sprite = ingredient.Sprite;
        _button.onClick.AddListener(() => chooseAction.Invoke(ingredient));
    }
}
