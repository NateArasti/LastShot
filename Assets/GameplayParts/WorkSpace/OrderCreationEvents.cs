using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class OrderCreationEvents : MonoBehaviour
{
    public static OrderCreationEvents Instance { get; private set; }

    [SerializeField] private WorkSpaceClocks _clocks;
    [SerializeField] private GlassSpaceFix _glassSpace;
    [SerializeField] private RectTransform _glassPivot;
    [SerializeField] private ItemContent _itemContent;
    [SerializeField] private Instrument[] _availableInstruments;
   
    [SerializeField] private Drink _drink;

    private void Start()
    {
        Instance = this;
        StartCreatingDrink(_drink);
    }

    public void StartCreatingDrink(Drink drink)
    {
        var glass = Instantiate(drink.DrinkReceipt.GlassPrefab, _glassPivot);
        glass.transform.SetAsFirstSibling();
        _glassSpace.SetGlassSpaceSize(glass.GetComponent<Glass>().GetTopPosition());
        var receipt = drink.DrinkReceipt;
        var ingredients = receipt.Ingredients;
        _itemContent.FillContent(ItemContent.ItemType.Alcohol,
            ingredients.Where(ingredient => 
                    ingredient.Data.Class == Ingredient.IngredientInfoData.ClassType.Alcohol
                    ));
        _itemContent.FillContent(ItemContent.ItemType.Ingredient,
            ingredients.Where(ingredient => 
                    ingredient.Data.Class == Ingredient.IngredientInfoData.ClassType.Ingredient
                    ));
        _clocks.StartClock(receipt.ApproximateTimeOfMaking + Random.Range(-0.25f, 0.25f) * receipt.ApproximateTimeOfMaking);

    }
}
