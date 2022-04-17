using UnityEngine;

[CreateAssetMenu(fileName = "SimpleDatabase", menuName = "Data/DatabaseManager")]
public class DatabaseManager : ScriptableObject
{
    private static DatabaseManager _instance;
    [Header("Databases")]
    [SerializeField] private IngredientDatabase _alcoholDatabase;
    [SerializeField] private IngredientDatabase _additionalIngredientsDatabase;
    [SerializeField] private CharacterDatabase _storyCharacterDatabaseDatabase;
    [SerializeField] private DrinkDatabase _drinkDatabase;

    public static IngredientDatabase AlcoholDatabase => _instance._alcoholDatabase;
    public static IngredientDatabase AdditionalIngredientDatabase => _instance._additionalIngredientsDatabase;
    public static CharacterDatabase CharacterDatabase => _instance._storyCharacterDatabaseDatabase;
    public static DrinkDatabase DrinkDatabase => _instance._drinkDatabase;

    public void SetAsInstance() => _instance = this;
}