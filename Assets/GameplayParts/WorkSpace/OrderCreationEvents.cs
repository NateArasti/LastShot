using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrderCreationEvents : MonoBehaviour
{
    public static OrderCreationEvents Instance { get; private set; }

    [SerializeField] private WorkSpaceClocks _clocks;
    [SerializeField] private BarShelf _barShelf;
    [SerializeField] private SpoonMix _spoonMix;
    [SerializeField] private GlassSpaceFix _glassSpace;
    [SerializeField] private RectTransform _glassPivot;
    [SerializeField] private ItemContent _itemContent;
   
    [SerializeField] private Drink _drink;

    private Glass _spawnedGlass;

    private void Start()
    {
        Instance = this;
        StartCreatingDrink(_drink);
    }

    public void StartCreatingDrink(Drink drink)
    {
        var glass = Instantiate(drink.DrinkReceipt.GlassPrefab, _glassPivot);
        glass.transform.SetAsFirstSibling();
        _spawnedGlass = glass.GetComponent<Glass>();
        _glassSpace.SetGlassSpaceSize(_spawnedGlass.GetTopPosition());
        _clocks.StartClock(drink.DrinkReceipt.ApproximateTimeOfMaking + 
                           Random.Range(-0.25f, 0.25f) * drink.DrinkReceipt.ApproximateTimeOfMaking);
        _barShelf.gameObject.SetActive(true);
        _barShelf.ShuffleIngredients();
    }

    public void SpawnIngredients(IReadOnlyCollection<Ingredient> ingredients)
    {
        _itemContent.ClearContent(ItemContent.ItemType.Alcohol);
        _itemContent.ClearContent(ItemContent.ItemType.Ingredient);

        _itemContent.FillContent(ItemContent.ItemType.Alcohol,
            ingredients.Where(ingredient =>
                ingredient.Data.Class == Ingredient.IngredientInfoData.ClassType.Alcohol
            ));
        _itemContent.FillContent(ItemContent.ItemType.Ingredient,
            ingredients.Where(ingredient =>
                ingredient.Data.Class == Ingredient.IngredientInfoData.ClassType.Ingredient
            ));
    }

    public void SwitchToSpoonMixEvent()
    {
        _spoonMix.gameObject.SetActive(true);
        _spoonMix.StartMixing(_spawnedGlass.LiquidRenderer);
    }

    public void SwitchToSpoonLayerEvent()
    {

    }

    public void SwitchToShakerEvent()
    {

    }
}
