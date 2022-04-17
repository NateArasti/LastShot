using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStorage", menuName = "Data/PlayerStorage")]
public class PlayerStorage : ScriptableObject
{
    private static PlayerStorage _instance;

    [SerializeField] private List<StorageItem> _storageItems = new();
    [SerializeField] private MoneyManager _moneyManager = new();
    private Dictionary<Ingredient, float[]> _volumes = new();

    public static MoneyManager MoneyData => _instance._moneyManager;

    public void SetAsInstance() => _instance = this;

    private void OnValidate()
    {
        RefillDictionary();
    }

    public static bool TryGetIngredientSumQuantity(Ingredient ingredient, out float quantity)
    {
        if (_instance._volumes.ContainsKey(ingredient))
        {
            quantity = _instance._volumes[ingredient].Sum();
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
                ? StorageItem.GetRandomValues(ingredient.Data.BuyQuantityStep)
                : new[] {ingredient.Data.BuyQuantityStep },
            DropIngredient _ => putRandomNumbers
                ? StorageItem.GetRandomValues(ingredient.Data.BuyQuantityStep, true)
                : new[] {ingredient.Data.BuyQuantityStep },
            _ => storageItem._volumes
        };
        _storageItems.Add(storageItem);

    }

    [System.Serializable]
    private struct StorageItem
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
