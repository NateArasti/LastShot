using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollContentFiller))]
public class DrinkInfoPage : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _tags;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _textReceipt;
    [SerializeField] private IngredientIcon _ingredientIconPrefab;
    [SerializeField] private UnityEvent<Ingredient> _ingredientChoose;
    [SerializeField] private UnityEvent<Instrument> _instrumentChoose;
    [SerializeField] private UnityEvent _onDataSet;
    private ScrollContentFiller _contentFiller;

    private void Awake()
    {
        _contentFiller = GetComponent<ScrollContentFiller>();
    }

    public void SetData(Drink drink)
    {
        var infoData = drink.InfoData;
        _icon.sprite = infoData.ObjectSprite;
        _name.text = infoData.Name;
        _tags.text = string.Join(" / ", infoData.Tags.Select(drinkTag => drinkTag.ToString()));
        _description.text = infoData.Description;
        _textReceipt.text = infoData.TextReceipt;

        _contentFiller.FillContent(
            _ingredientIconPrefab, 
            drink.DrinkReceipt.Instruments,
            (icon, instrument) => icon.SetData(instrument, arg0 => _instrumentChoose.Invoke(arg0)), 
            true);
        _contentFiller.FillContent(
            _ingredientIconPrefab, 
            drink.DrinkReceipt.Ingredients,
            (icon, ingredient) => icon.SetData(ingredient, arg0 => _ingredientChoose.Invoke(arg0)));
        _onDataSet.Invoke();
    }

    public void ForceUpdateScroll(float delta)
    {
        transform.localPosition += Vector3.up * delta;
    }
}
