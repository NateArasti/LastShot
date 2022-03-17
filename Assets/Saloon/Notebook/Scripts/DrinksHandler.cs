using UnityEngine;
using UnityEngine.Events;

public class DrinksHandler : MonoBehaviour
{
    [SerializeField] private RectTransform _spawnPivot;
    [SerializeField] private GameObject _drinkPanelPrefab;
    [SerializeField] private UnityEvent<Drink> _onChoose;

    private void Start()
    {
        foreach(var drink in DatabaseManager.DrinkDatabase.GetObjectsCollection())
        {
            Instantiate(_drinkPanelPrefab, _spawnPivot)
                .GetComponent<DrinkInfoPage>()
                .SetInfo(drink, chosenDrink => _onChoose.Invoke(chosenDrink));
        }
    }
}
