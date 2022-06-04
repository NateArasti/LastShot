using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PourIngredient", menuName = "Data/DataItem/PourIngredient")]
public class PourIngredient : Ingredient
{
    [Space(20f)]
    [SerializeField] private Color _mainColor;
    [SerializeField] private float _maxDelayBetweenDrops = 0.05f;

    protected override IngredientTypeData.IngredientType Type => IngredientTypeData.IngredientType.Pour;

    public override bool CanPlaceInThisSpace(ItemSpace.ItemSpaceType type) =>
        type == ItemSpace.ItemSpaceType.SideObject;

    public override GameObject SpawnWorkItem(Transform container)
    {
        var item = Instantiate(IngredientTypeData.GetPrefab(Type), container);
        item.GetComponent<PourItem>().SetItem(WorkSprite, _mainColor, _maxDelayBetweenDrops);

        return item;
    }
}
