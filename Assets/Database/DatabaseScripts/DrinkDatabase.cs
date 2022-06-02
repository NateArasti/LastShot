using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "DrinkDatabase", menuName = "Data/Database/DrinkDatabase")]
public class DrinkDatabase : SpriteDatabase<Drink>
{
    [SerializeField] private Drink _anythingDrink;

    public string AnythingKeyName => _anythingDrink.KeyName;

    public void FillIngredientContainingLists()
    {
        var drinksCollection = GetObjectsCollection();
        drinksCollection.ForEachAction(
            drink =>
            {
                drink.DrinkReceipt.Ingredients.ForEachAction(
                    ingredient =>
                    {
                        ingredient.Data.ContainingDrinks.Clear();
                    });
            });
        drinksCollection.ForEachAction(
            drink =>
            {
                drink.DrinkReceipt.Ingredients.ForEachAction(
                    ingredient =>
                    {
                        ingredient.Data.ContainingDrinks.Add(drink);
                    });
#if UNITY_EDITOR
                EditorUtility.SetDirty(drink);
#endif
            });
        drinksCollection.ForEachAction(
            drink =>
            {
                drink.DrinkReceipt.Instruments.ForEachAction(
                    instrument =>
                    {
                        instrument.ContainingDrinks.Clear();
                    });
            });
        drinksCollection.ForEachAction(
            drink =>
            {
                drink.DrinkReceipt.Instruments.ForEachAction(
                    instrument =>
                    {
                        instrument.ContainingDrinks.Add(drink);
                    });
#if UNITY_EDITOR
                EditorUtility.SetDirty(drink);
#endif
            });
        Debug.Log("Successfully filled ingredients containing lists");
    }

    public override bool TryGetValue(string keyName, out Drink value)
    {
        if (keyName == _anythingDrink.KeyName)
        {
            value = _anythingDrink;
            return true;
        }
        return base.TryGetValue(keyName, out value);
    }
}