using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ScrollContentFiller))]
public class DrinkScroll : MonoBehaviour
{
    [SerializeField] private DrinkPanel _panelPrefab;
    [SerializeField] private UnityEvent<Drink> _chooseEvent;
    private ScrollContentFiller _contentFiller;
    private IReadOnlyDictionary<Drink, DrinkPanel> _spawnedDrinksPanels;

    private void Awake()
    {
        _contentFiller = GetComponent<ScrollContentFiller>();
    }

    private void Start()
    {
        _spawnedDrinksPanels = _contentFiller.FillContent(
            _panelPrefab,
            DatabaseManager.DrinkDatabase.GetObjectsCollection(), 
            (panel, drink) => panel.SetData(drink, _chooseEvent));
    }

    public void ManuallyChooseDrink(Drink drink)
    {
        if(_spawnedDrinksPanels == null) Start();
        if (!_spawnedDrinksPanels.ContainsKey(drink))
        {
            Debug.LogError($"Didn't spawned drink {drink.InfoData.Name}");
            return;
        }
        _spawnedDrinksPanels[drink].ManuallyChoose();
    }
}
