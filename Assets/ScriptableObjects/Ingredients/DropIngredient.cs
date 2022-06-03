using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "DropIngredient", menuName = "Data/DataItem/DropIngredient")]
public class DropIngredient : Ingredient
{
    [Space(20f)]
    [SerializeField] private float _mass;
    [SerializeField] private bool _float;

    protected override IngredientTypeData.IngredientType Type => IngredientTypeData.IngredientType.Drop;

    public override bool CanPlaceInThisSpace(ItemSpace.ItemSpaceType type)
    {
        return type is ItemSpace.ItemSpaceType.Glass or ItemSpace.ItemSpaceType.Garnish;
    }

    public override GameObject SpawnWorkItem(Transform container)
    {
        var item = Instantiate(IngredientTypeData.GetPrefab(Type), container);
        item.GetComponent<DropItem>().SetItem(Sprite, _mass, _float);
        item.GetComponent<Image>().SetNativeSize();
        item.GetComponent<BoxCollider2D>().size = item.GetComponent<RectTransform>().sizeDelta;
        return item;
    }
}
