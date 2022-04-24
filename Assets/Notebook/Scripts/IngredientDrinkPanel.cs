using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class IngredientDrinkPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Button _button;

    public void SetData(Drink drink, UnityAction<Drink> chooseAction)
    {
        _name.text = drink.InfoData.Name;
        _button.onClick.AddListener(() => chooseAction.Invoke(drink));
    }
}
