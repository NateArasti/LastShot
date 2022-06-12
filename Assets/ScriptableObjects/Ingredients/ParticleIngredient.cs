using UnityEngine;

[CreateAssetMenu(fileName = "ParticleIngredient", menuName = "Data/DataItem/ParticleIngredient")]
public class ParticleIngredient : Ingredient
{
    [SerializeField] private Color _color;
    [SerializeField] private float _dropCount;

    protected override IngredientTypeData.IngredientType Type => IngredientTypeData.IngredientType.ParticleDrop;

    public override bool CanPlaceInThisSpace(ItemSpace.ItemSpaceType type) =>
        type == ItemSpace.ItemSpaceType.Glass || type == ItemSpace.ItemSpaceType.Garnish && Data.Garnishable;

    public override GameObject SpawnWorkItem(Transform container)
    {
        var action = new OrderAction.IngredientAddAction(false)
        {
            Ingredient = this,
            Quantity = _dropCount
        };
        OrderCreationEvents.Instance.OrderActionsTracker.AddAction(action);
        var item = Instantiate(IngredientTypeData.GetPrefab(Type), container);
        item.GetComponent<ParticleDropItem>().SetItem(_color);
        return item;
    }
}
