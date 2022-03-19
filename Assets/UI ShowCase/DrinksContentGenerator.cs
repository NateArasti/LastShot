using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DrinksContentGenerator : MonoBehaviour
{
    [SerializeField] private DrinkDataHandler _drinkInfoPanel;
    [SerializeField] private UnityEvent _onChosenDrink;
    private readonly List<Drink> _spawnedDrinks = new();

    private void Awake()
    {
        var drinks = DatabaseManager.DrinkDatabase.GetObjectsCollection();
        foreach (var drink in drinks)
        {
            _spawnedDrinks.Add(drink);
            var drinkDataHandler = Instantiate(_drinkInfoPanel, transform);
            drinkDataHandler.SetDrinkData(drink, _spawnedDrinks.Count - 1);
            drinkDataHandler.OnDrinkChoose.AddListener(ChooseDrink);
        }
    }

    private void ChooseDrink(int index)
    {
        var drink = _spawnedDrinks[index];
        //something
        _onChosenDrink.Invoke();
    }
}
