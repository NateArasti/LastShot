using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStorage", menuName = "Data/PlayerStorage")]
public class PlayerStorage : ScriptableObject
{
    [Tooltip("When toggled, makes this variant a main instance")]
    [SerializeField] private bool _isInstance;
    [SerializeField] private List<StorageItem> _storageItems = new List<StorageItem>();
    private Dictionary<Ingredient, float[]> _volumes = new Dictionary<Ingredient, float[]>();
    [SerializeField] private MoneyManager _moneyManager = new MoneyManager();
    public static PlayerStorage Instance { get; private set; }

    public MoneyManager MoneyData => _moneyManager;

    private void OnValidate()
    {
        if (_isInstance && Instance != this)
        {
            if(Instance != null)
                Instance._isInstance = false;
            Instance = this;
        }
        RefillDictionary();
    }

    public bool TryGetIngredientSumQuantity(Ingredient ingredient, out float quantity)
    {
        if (_volumes.ContainsKey(ingredient))
        {
            quantity = _volumes[ingredient].Sum();
            return true;
        }

        quantity = default;
        return false;
    }

    private void RefillDictionary()
    {
        _volumes = new Dictionary<Ingredient, float[]>();
        foreach (var storageItem in _storageItems)
        {
            _volumes[storageItem._ingredient] = storageItem._volumes;
        }
    }

    private void OnEnable()
    {
        _isInstance = true;
        OnValidate();
    }

    public void Distinct()
    {
        var newList = new List<StorageItem>();
        var storageItemSet = new HashSet<Ingredient>();
        foreach (var storageItem in _storageItems)
        {
            if (storageItemSet.Contains(storageItem._ingredient)) continue;
            storageItemSet.Add(storageItem._ingredient);
            newList.Add(storageItem);
        }

        _storageItems = newList;
        OnValidate();
    }

    public void LoadFromDatabase(bool putRandomNumbers)
    {
        var additionalIngredients = DatabaseManager.AdditionalIngredientDatabase.GetObjectsCollection();
        var alcohols = DatabaseManager.AlcoholDatabase.GetObjectsCollection();
        _storageItems = new List<StorageItem>(additionalIngredients.Count + alcohols.Count);
        foreach (var ingredient in additionalIngredients) AddStorageItemFromIngredient(ingredient, putRandomNumbers);
        foreach (var ingredient in alcohols) AddStorageItemFromIngredient(ingredient, putRandomNumbers);

        Debug.Log($"Loaded Objects for {name} successfully");
    }

    private void AddStorageItemFromIngredient(Ingredient ingredient, bool putRandomNumbers)
    {
        var storageItem = new StorageItem(ingredient);
        storageItem._volumes = ingredient switch
        {
            PourIngredient _ => putRandomNumbers
                ? StorageItem.GetRandomValues(ingredient.Data.buyQuantityStep)
                : new[] {ingredient.Data.buyQuantityStep},
            DropIngredient _ => putRandomNumbers
                ? StorageItem.GetRandomValues(ingredient.Data.buyQuantityStep, true)
                : new[] {ingredient.Data.buyQuantityStep},
            _ => storageItem._volumes
        };
        _storageItems.Add(storageItem);

    }

    [System.Serializable]
    struct StorageItem
    {
        public Ingredient _ingredient;
        public float[] _volumes;

        public StorageItem(Ingredient ingredient) : this()
        {
            _ingredient = ingredient;
        }

        public static float[] GetRandomValues(float maxVolume, bool volumesShouldBeInt = false)
        {
            var volumes = new float[Random.Range(1, 10)];
            for (var i = 0; i < volumes.Length; i++)
            {
                if (volumesShouldBeInt)
                    volumes[i] = Random.Range(1, maxVolume);
                else
                    volumes[i] = Random.Range(0.25f, 1f) * maxVolume;
            }
            return volumes;
        }
    }
}
