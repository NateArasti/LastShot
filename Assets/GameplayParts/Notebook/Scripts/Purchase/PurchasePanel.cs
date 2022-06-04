using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Image))]
public class PurchasePanel : MonoBehaviour
{
    [SerializeField] private Sprite _discountSprite;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _cost;
    [SerializeField] private TMP_Text _discountCost;
    [SerializeField] private TMP_Text _inStorageText;
    [SerializeField] private TMP_Text _sumText;
    [SerializeField] private TMP_Text _totalV;
    [SerializeField] private Slider _buySlider;
    [SerializeField] private Button _infoButton;
    private Image _panelImage;
    private int _currentTotalCount;
    private int _costPerObject;
    private float _buyStep;
    private Ingredient _ingredient;

    public int TotalCost => _currentTotalCount * _costPerObject;

    private void Awake()
    {
        _panelImage = GetComponent<Image>();
    }

    public void SetData(Ingredient ingredient, UnityAction<Ingredient> onInfoChoose)
    {
        _ingredient = ingredient;

        _infoButton.onClick.AddListener(() => onInfoChoose.Invoke(ingredient));
        _costPerObject = ingredient.Data.CostPerObject;
        _buyStep = ingredient.Data.BuyQuantityStep;
        if (Random.value < 0.2f)
        {
            _panelImage.sprite = _discountSprite;
            var color = _cost.color;
            color.a = 0.15f;
            _cost.color = color;
            _cost.fontStyle = FontStyles.Strikethrough;
            _costPerObject = Mathf.CeilToInt(_costPerObject * 0.5f);
            _discountCost.enabled = true;
        }
        _cost.text = $"{_costPerObject}$";
        _discountCost.text = $"{_costPerObject}$";

        _icon.sprite = ingredient.Icon;
        _name.text = ingredient.Data.Name;
        if (PlayerStorage.TryGetIngredientSumQuantity(ingredient, out var quantity))
        {
            _inStorageText.text = $"В салуне {quantity} {ingredient.Data.GetPurchaseSuffix()}";
        }

        _buySlider.maxValue =
            Mathf.FloorToInt((float) PlayerStorage.MoneyData.TotalMoney / _costPerObject);

        _buySlider.onValueChanged.AddListener(OnSliderValueChange);

        _buySlider.onValueChanged.AddListener(_ =>
        {
            _sumText.text = $"{_buySlider.value * _costPerObject}$";
            _totalV.text = $"{_buySlider.value * _buyStep} {ingredient.Data.GetPurchaseSuffix()}";
        });

        _buySlider.onValueChanged.Invoke(0);
    }

    private void OnSliderValueChange(float value)
    {
        var addValue = (int) value - _currentTotalCount;
        if (addValue * _costPerObject > PlayerStorage.MoneyData.CurrentMoney)
        {
            addValue = Mathf.FloorToInt((float) PlayerStorage.MoneyData.CurrentMoney / _costPerObject);
        }
        _currentTotalCount += addValue;
        _buySlider.value = _currentTotalCount;
        PlayerStorage.MoneyData.ChangeBill(_ingredient, _currentTotalCount * _costPerObject);
        PlayerStorage.MoneyData.CurrentMoney -= addValue * _costPerObject;
    }
}
