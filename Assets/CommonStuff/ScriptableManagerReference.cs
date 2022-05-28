using UnityEngine;

public class ScriptableManagerReference : MonoBehaviour
{
    [SerializeField] private DatabaseManager _databaseManager;
    [SerializeField] private PlayerStorage _playerStorage;
    [SerializeField] private IngredientTypeData _ingredientTypeData;

    private void Awake()
    {
        _ingredientTypeData.SetInstance();
        //_databaseManager.SetAsInstance();
        //_playerStorage.SetAsInstance();
    }
}
