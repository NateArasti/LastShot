using UnityEngine;
using UnityEngine.Events;

public class OrderCreationEvents : MonoBehaviour
{
    public static OrderCreationEvents Instance { get; private set; }

    [SerializeField] private RectTransform _glassPivot;
    [SerializeField] private ItemContent _itemContent;
    [SerializeField] private GameObject _liquidPrefab;
    [SerializeField] private Drink _drink;

    private void Start()
    {
        Instance = this;
        StartCreatingDrink(_drink);
    }

    public void StartCreatingDrink(Drink drink)
    {
        Instantiate(drink.Receipt.glassPrefab, _glassPivot).transform.SetAsFirstSibling();
        var receipt = drink.Receipt;
        _itemContent.FillContent(ItemContent.ItemType.Alcohol, receipt.alcohols);
        _itemContent.FillContent(ItemContent.ItemType.Ingredient, receipt.ingredients);
        _itemContent.FillContent(ItemContent.ItemType.Instrument, receipt.instruments);
    }

    public Liquid SpawnStartLiquid(RectTransform maskRect)
    {
        var newStartLiquid = Instantiate(_liquidPrefab, _liquidPrefab.transform.parent).GetComponent<Liquid>();
        newStartLiquid.gameObject.SetActive(true);
        return newStartLiquid.SpawnStartLiquid(maskRect);
    }
}
