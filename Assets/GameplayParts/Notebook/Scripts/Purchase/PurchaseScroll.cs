using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PurchaseScroll : MonoBehaviour
{
    [SerializeField] private ScrollContentFiller _alcoholContentFiller;
    [SerializeField] private PurchasePanel _alcoholPurchasePanel;
    [SerializeField] private ScrollContentFiller _ingredientContentFiller;
    [SerializeField] private PurchasePanel _ingredientPurchasePanel;
    [SerializeField] private UnityEvent<Ingredient> _onInfoChoose;

    private readonly List<(Ingredient, PurchasePanel)> _purchaseData = new();

    public IReadOnlyCollection<(Ingredient, PurchasePanel)> PurchaseCollection => _purchaseData;

    private void Start()
    {
        _alcoholContentFiller.FillContent
            (
            _alcoholPurchasePanel,
            DatabaseManager.AlcoholDatabase.GetObjectsCollection(),
            (panel, ingredient) =>
            {
                if (ingredient.Data.IgnorePurchase)
                {
                    Destroy(panel.gameObject);
                    return;
                }
                panel.SetData(ingredient, _onInfoChoose.Invoke);
            }).ForEachAction
                (
                    pair => _purchaseData.Add((pair.Key, pair.Value))
                );

        _ingredientContentFiller.FillContent
            (
            _ingredientPurchasePanel,
            DatabaseManager.AdditionalIngredientDatabase.GetObjectsCollection(),
            (panel, ingredient) =>
            {
                if (ingredient.Data.IgnorePurchase)
                {
                    Destroy(panel.gameObject);
                    return;
                }
                panel.SetData(ingredient, _onInfoChoose.Invoke);
            }).ForEachAction
                (
                    pair => _purchaseData.Add((pair.Key, pair.Value))
                );
    }
}
