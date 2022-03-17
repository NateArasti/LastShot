using UnityEditor;
using UnityEngine;

public abstract class Ingredient : ScriptableObject, IWorkItem, IDatabaseObject
{
    [SerializeField] private string _keyName;
    [SerializeField] private Sprite _sprite;

    protected abstract IngredientTypeData.IngredientType Type { get; }

    [Header("Buy Params")]
    private string _name;
    [SerializeField] private float _buyQuantityStep;
    [SerializeField] private int _costPerObject;

    public abstract bool CanPlaceInThisSpace(ItemSpace.ItemSpaceType type);

    public abstract GameObject SpawnWorkItem(Transform container);

    public string KeyName => _keyName;

    public (string name, float buyQuantityStep, int costPerObject) Data => (_name, _buyQuantityStep, _costPerObject);

    public Sprite Sprite
    {
        get => _sprite;
        set
        {
            if (EditorApplication.isPlaying) return;
            _sprite = value;
        }
    }

    public void WriteData(string[] paramsLine)
    {
        if (paramsLine.Length != 4) throw new UnityException($"Wrong paramsLine {paramsLine}");
        _name = paramsLine[1];
        _costPerObject = int.Parse(paramsLine[2]);
        _buyQuantityStep = float.Parse(paramsLine[3]);
    }

    public bool TakeMousePosition() => Type == IngredientTypeData.IngredientType.Drop;
}
