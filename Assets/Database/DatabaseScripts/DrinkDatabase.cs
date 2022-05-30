using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "DrinkDatabase", menuName = "Data/Database/DrinkDatabase")]
public class DrinkDatabase : Database<Drink>
{
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
}