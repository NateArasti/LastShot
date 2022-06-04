using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "DropIngredient", menuName = "Data/DataItem/DropIngredient")]
public class DropIngredient : Ingredient
{
    [Space(20f)]
    [SerializeField] private float _mass;
    [SerializeField] private float _volume;
    [SerializeField] private int _additionalSpawnCount;
    [SerializeField] private float _rigidbodyTurnOffDelay;

    public override Sprite GarnishSprite => WorkSprite;

    protected override IngredientTypeData.IngredientType Type => 
            IngredientTypeData.IngredientType.Drop;

    public override bool CanPlaceInThisSpace(ItemSpace.ItemSpaceType type) => 
        type == ItemSpace.ItemSpaceType.Glass || type == ItemSpace.ItemSpaceType.Garnish && Data.Garnishable;

    public override GameObject SpawnWorkItem(Transform container)
    {
        var item = Instantiate(IngredientTypeData.GetPrefab(Type), container);
        item.GetComponent<DropItem>()
            .SetItem(WorkSprite, _mass, _volume, _additionalSpawnCount, _rigidbodyTurnOffDelay);
        item.GetComponent<Image>().SetNativeSize();
        item.GetComponent<BoxCollider2D>().size = item.GetComponent<RectTransform>().sizeDelta;
        return item;
    }
}
