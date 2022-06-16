using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BarShelf : MonoBehaviour
{
    [SerializeField] private ShelfChosenIngredient _chosenIngredientPrefab;
    [SerializeField] private Transform _chosenIngredientParent;
    [SerializeField] private UnityEvent<IReadOnlyCollection<Ingredient>> _onSwitch;
    [SerializeField] private int _chosenIngredientCap = 12;
    private ShelfIngredient[] _shelfIngredients;
    private readonly HashSet<int> _availablePositions = new();

    private readonly HashSet<Ingredient> _chosenIngredients = new();
    private readonly Dictionary<Ingredient, int> _shelfIngredientsIndexes = new();

    private List<Ingredient> _ingredients;
    private HashSet<Button> _chosenIngredientButtons = new();

    private void Awake()
    {
        _shelfIngredients = GetComponentsInChildren<ShelfIngredient>(true);
        for (var i = 0; i < _shelfIngredients.Length; i++)
        {
            _availablePositions.Add(i);
        }
    }

    public void ShuffleIngredients()
    {
        if(_ingredients == null)
        {
            _shelfIngredientsIndexes.Clear();
            _ingredients = DatabaseManager.AlcoholDatabase.GetObjectsCollection().ToList();
            _ingredients.AddRange(DatabaseManager.AdditionalIngredientDatabase.GetObjectsCollection()
                .Where(ingredient => !ingredient.Data.IgnorePurchase));
            var availablePositions = new HashSet<int>(_availablePositions);
            foreach (var ingredient in _ingredients)
            {
                var index = availablePositions.GetRandomObject();
                availablePositions.Remove(index);
                _shelfIngredients[index].gameObject.SetActive(true);
                _shelfIngredients[index].SetIngredient(ingredient, ChooseIngredient);
                _shelfIngredientsIndexes.Add(ingredient, index);
            }
        }
    }

    public void ClearShelf()
    {
        var chosen = _chosenIngredientButtons.ToArray();
        foreach (var ingredient in chosen)
        {
            ingredient.onClick.Invoke();
        }

    }

    public void ChooseIngredient(Ingredient ingredient, GameObject ingredientGameObject)
    {
        if(_chosenIngredients.Count == _chosenIngredientCap) return;
        ingredientGameObject.SetActive(false);
        var chosen = Instantiate(_chosenIngredientPrefab, _chosenIngredientParent);
        chosen.SetIngredient(ingredient, UnChooseIngredient);
        _chosenIngredientButtons.Add(chosen.GetComponent<Button>());
        _chosenIngredients.Add(ingredient);
    }

    public void UnChooseIngredient(Ingredient ingredient)
    {
        _chosenIngredients.Remove(ingredient);
        _shelfIngredients[_shelfIngredientsIndexes[ingredient]].gameObject.SetActive(true);
    }

    public void SwitchToWorkSpace() => _onSwitch.Invoke(_chosenIngredients);
}
