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
            });
        Debug.Log("Successfully filled ingredients containing lists");
    }
}