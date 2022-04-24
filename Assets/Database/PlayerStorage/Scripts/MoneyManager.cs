using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

[Serializable]
public class MoneyManager
{
    public MoneyManager()
    {
        PutRandomNumbers();
    }

    public void PutRandomNumbers()
    {
        var rand = new Random();
        _stock = rand.Next(100, 500);
        _gainedOnCurrentDay = rand.Next(30, 100);
    }

    [SerializeField] private int _stock;
    [SerializeField] private int _gainedOnCurrentDay;
    [SerializeField] private int _currentMoney;

    private Dictionary<Ingredient, int> _currentBill = new();

    public int TotalMoney => _stock + _gainedOnCurrentDay;
    public int Stock => _stock;
    public int GainedOnCurrentDay => _gainedOnCurrentDay;

    public int CurrentMoney
    {
        get => _currentMoney;
        set => _currentMoney = value;
    }

    public IReadOnlyList<(string name, int cost)> CurrentBill =>
        _currentBill.Select(pair => (pair.Key.Data.Name, pair.Value)).ToList();

    public void ChangeBill(Ingredient ingredient, int newCost)
    {
        _currentBill[ingredient] = newCost;
    }

    public void ResetCurrentMoney() => CurrentMoney = TotalMoney;
}
