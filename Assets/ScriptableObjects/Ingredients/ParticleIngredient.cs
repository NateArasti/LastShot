using UnityEngine;

[CreateAssetMenu(fileName = "ParticleIngredient", menuName = "Data/DataItem/ParticleIngredient")]
public class ParticleIngredient : Ingredient
{
    [SerializeField] private Color _color;

    protected override IngredientTypeData.IngredientType Type => IngredientTypeData.IngredientType.ParticleDrop;

    public override bool CanPlaceInThisSpace(ItemSpace.ItemSpaceType type) =>
        type == ItemSpace.ItemSpaceType.Glass || type == ItemSpace.ItemSpaceType.Garnish && Data.Garnishable;

    public override GameObject SpawnWorkItem(Transform container)
    {
        var item = Instantiate(IngredientTypeData.GetPrefab(Type), container);
        item.GetComponent<ParticleDropItem>().SetItem(_color);
        return item;
    }
}
