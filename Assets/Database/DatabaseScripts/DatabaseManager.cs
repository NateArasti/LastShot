using UnityEngine;

[CreateAssetMenu(fileName = "SimpleDatabase", menuName = "Data/DatabaseManager")]
public class DatabaseManager : ScriptableObject
{
    private static DatabaseManager _instance;
    [Tooltip("When toggled, makes this variant a main instance")]
    [SerializeField] private bool _isInstance;
    [Header("Databases")]
    [SerializeField] private IngredientDatabase _alcoholDatabase;
    [SerializeField] private IngredientDatabase _additionalIngredientsDatabase;
    [SerializeField] private CharacterDatabase _characterDatabaseDatabase;
    [SerializeField] private DrinkDatabase _drinkDatabase;

    public static IngredientDatabase AlcoholDatabase => _instance._alcoholDatabase;
    public static IngredientDatabase AdditionalIngredientDatabase => _instance._additionalIngredientsDatabase;
    public static CharacterDatabase CharacterDatabase => _instance._characterDatabaseDatabase;
    public static DrinkDatabase DrinkDatabase => _instance._drinkDatabase;

    private void OnValidate()
    {
        if (_isInstance && _instance != this)
        {
            if (_instance != null)
                _instance._isInstance = false;
            _instance = this;
        }
    }

    private void OnEnable()
    {
        _isInstance = true;
        OnValidate();
    }
}