using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(ScrollContentFiller))]
public class IngredientInfoPanel : MonoBehaviour
{
    [SerializeField] private Sprite _alcoholPanel;
    [SerializeField] private Sprite _ingredientPanel;
    [SerializeField] private Sprite _instrumentPanel;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private IngredientDrinkPanel _drinkPanelPrefab;
    [SerializeField] private UnityEvent<Drink> _drinkChoose;
    private Image _panelImage;
    private ScrollContentFiller _contentFiller;

    private void Awake()
    {
        _panelImage = GetComponent<Image>();
        _contentFiller = GetComponent<ScrollContentFiller>();
    }

    public void SetData(Ingredient ingredient)
    {
        _panelImage.sprite = ingredient.Data.Class == Ingredient.IngredientInfoData.ClassType.Alcohol ? 
            _alcoholPanel : 
            _ingredientPanel;
        _icon.sprite = ingredient.Data.ObjectSprite;
        _name.text = ingredient.Data.Name;
        _description.text = ingredient.Data.Description;

        _contentFiller.FillContent(
            _drinkPanelPrefab, 
            ingredient.Data.ContainingDrinks, 
            (panel, drink) => panel.SetData(drink, d => _drinkChoose.Invoke(d)),
            true);
    }

    public void SetData(Instrument instrument)
    {
        _panelImage.sprite = _instrumentPanel;
        _icon.sprite = instrument.Data.ObjectSprite;
        _name.text = instrument.Data.Name;
        _description.text = instrument.Data.Description;

        _contentFiller.FillContent(
            _drinkPanelPrefab,
            instrument.ContainingDrinks, 
            (panel, drink) => panel.SetData(drink, d => _drinkChoose.Invoke(d)),
            true);
    }
}
