using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class OrderCreationEvents : MonoBehaviour
{
    public static OrderCreationEvents Instance { get; private set; }

    [SerializeField] private RectTransform _glassPivot;
    [SerializeField] private ItemContent _itemContent;
   
    [SerializeField] private Drink _drink;

    private void Start()
    {
        Instance = this;
        StartCreatingDrink(_drink);
    }

    public void StartCreatingDrink(Drink drink)
    {
        Instantiate(drink.DrinkReceipt.GlassPrefab, _glassPivot).transform.SetAsFirstSibling();
        var receipt = drink.DrinkReceipt;
        _itemContent.FillContent(ItemContent.ItemType.Alcohol, receipt.Alcohols);
        _itemContent.FillContent(ItemContent.ItemType.Alcohol, receipt.Ingredients);
        //_itemContent.FillContent(ItemContent.ItemType.Instrument, receipt.instruments);
    }
}
