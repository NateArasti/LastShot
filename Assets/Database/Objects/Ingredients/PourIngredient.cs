using UnityEngine;

[CreateAssetMenu(fileName = "PourIngredient", menuName = "Data/DataItem/PourIngredient")]
public class PourIngredient : Ingredient
{
    [SerializeField] private Color _mainColor;
    [SerializeField] private Color _outlineColor;
    [SerializeField] private float _delayBetweenDrops = 0.05f;

    protected override IngredientTypeData.IngredientType Type => IngredientTypeData.IngredientType.Pour;

    public override bool CanPlaceInThisSpace(ItemSpace.ItemSpaceType type) =>
        type == ItemSpace.ItemSpaceType.SideObject ||
        type == ItemSpace.ItemSpaceType.List;

    public override GameObject SpawnWorkItem(Transform container)
    {
        var item = Instantiate(IngredientTypeData.GetPrefab(Type), container);
        item.GetComponent<PourItem>().SetItem(Sprite, (_mainColor, _outlineColor), _delayBetweenDrops);
        return item;
    }
}
