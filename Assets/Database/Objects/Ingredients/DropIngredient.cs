using UnityEngine;

[CreateAssetMenu(fileName = "DropIngredient", menuName = "Data/DataItem/DropIngredient")]
public class DropIngredient : Ingredient
{
    [SerializeField] private float _mass;
    [SerializeField] private bool _float;

    protected override IngredientTypeData.IngredientType Type => IngredientTypeData.IngredientType.Drop;

    public override bool CanPlaceInThisSpace(ItemSpace.ItemSpaceType type)
    {
        return type == ItemSpace.ItemSpaceType.Glass || type == ItemSpace.ItemSpaceType.Garnish ||
               type == ItemSpace.ItemSpaceType.List;
    }

    public override GameObject SpawnWorkItem(Transform container)
    {
        var item = Instantiate(IngredientTypeData.GetPrefab(Type), container);
        item.GetComponent<DropItem>().SetItem(Sprite, _mass, _float);
        return item;
    }
}
