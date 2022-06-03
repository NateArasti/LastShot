using UnityEngine;

[CreateAssetMenu(fileName = "IngredientTypeData", menuName = "Data/IngredientTypeData")]
public class IngredientTypeData : ScriptableSingleton
{
    public enum IngredientType
    {
        Pour,
        Drop,
    }

    private static IngredientTypeData _instance;
    
    [SerializeField] private TypePrefabData[] _datas;

    public override void SetAsInstance()
    {
        _instance = this;
    }

    public static GameObject GetPrefab(IngredientType type)
    {
        foreach(var data in _instance._datas)
        {
            if (data.Type == type)
                return data.Prefab;
        }
        Debug.LogError($"No such type {type} implemented");
        return null;
    }

    [System.Serializable]
    public struct TypePrefabData
    {
        [SerializeField] private IngredientType _type;
        [SerializeField] private GameObject _prefab;

        public IngredientType Type => _type;
        public GameObject Prefab => _prefab;
    }
}
