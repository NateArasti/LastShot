using UnityEngine;

public class ScriptableManagerReference : MonoBehaviour
{
    [SerializeField] private DatabaseManager _databaseManager;
    [SerializeField] private PlayerStorage _playerStorage;

    private void Awake()
    {
        _databaseManager.SetAsInstance();
        _playerStorage.SetAsInstance();
    }
}
