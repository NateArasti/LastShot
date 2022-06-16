using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrderCreationEvents : MonoBehaviour
{
    public static OrderCreationEvents Instance { get; private set; }

    [SerializeField] private WorkSpaceClocks _clocks;
    [SerializeField] private BarShelf _barShelf;
    [SerializeField] private ItemContent _itemContent;
    [SerializeField] private GlassSpaceFix _glassSpace;
    [SerializeField] private RectTransform _glassPivot;
    [Header("Items")]
    [SerializeField] private ItemSpace[] _itemSpaces;
    [Header("Instruments")]
    [SerializeField] private SpoonMix _spoonMix;

    private Glass _spawnedGlass;

    public OrderActionsTracker OrderActionsTracker { get; private set; }

    public bool DrinkInWork { get; private set; } 

    private void Awake()
    {
        Instance = this;
    }

    public void StartCreatingDrink(Drink drink)
    {
        OrderActionsTracker = new OrderActionsTracker();
        DrinkInWork = true;
        var glass = Instantiate(drink.DrinkReceipt.GlassPrefab, _glassPivot);
        glass.transform.SetAsFirstSibling();
        _spawnedGlass = glass.GetComponent<Glass>();
        _glassSpace.SetGlassSpaceSize(_spawnedGlass.GetTopPosition());
        _clocks.StartClock(drink.DrinkReceipt.ApproximateTimeOfMaking + 
                           Random.Range(-0.25f, 0.25f) * drink.DrinkReceipt.ApproximateTimeOfMaking);
        _barShelf.gameObject.SetActive(true);
        _barShelf.ShuffleIngredients();
    }

    public void EndCreation()
    {
        _itemSpaces.ForEachAction(itemSpace => itemSpace.ClearSpace());
        _barShelf.ClearShelf();
        DrinkInWork = false;
        Destroy(_spawnedGlass.gameObject);
        GameStateManager.ExitWorkSpace();
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
        Debug.LogError("SPOON LAYERING NOT IMPLEMENTED");
    }

    public void SwitchToShakerEvent()
    {

    }
}