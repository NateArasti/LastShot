using System.Drawing.Printing;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientTypeData", menuName = "Data/IngredientTypeData")]
public class IngredientTypeData : ScriptableObject
{
    public enum IngredientType
    {
        Pour,
        Drop,
    }

    private static IngredientTypeData _instance;

    [Tooltip("When toggled, makes this variant a main instance")]
    [SerializeField] private bool _isInstance;
    [SerializeField] private TypePrefabData[] _datas;

    public void SetInstance()
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

    private void OnEnable()
    {
        _instance = this;
    }

    private void OnValidate()
    {
        if (_isInstance)
            _instance = this;
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
