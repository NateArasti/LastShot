using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    [SerializeField] private float _stock;
    [SerializeField] private float _gainedOnCurrentDay;
    [SerializeField] private float _currentMoney;

    private Dictionary<Ingredient, int> _currentBill = new();
    private Dictionary<Drink, float> _dayOrders = new();

    public float TotalMoney => _stock + _gainedOnCurrentDay;
    public float Stock => _stock;
    public float GainedOnCurrentDay => _gainedOnCurrentDay;

    public float CurrentMoney
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

    public void AddOrderMoney(Drink drink, CharacterGuestGrade grade)
    {
        float earned;
        switch (grade)
        {
            case CharacterGuestGrade.Excellent:
                earned = drink.InfoData.Cost * 1f;
                break;
            case CharacterGuestGrade.Good:
                earned = drink.InfoData.Cost * 0.5f;
                break;
            case CharacterGuestGrade.Bad:
                earned = drink.InfoData.Cost * 0f;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(grade), grade, null);
        }
        _dayOrders.Add(drink, earned);
        _gainedOnCurrentDay += earned;
    }

    public string EarnedData()
    {
        var result = new StringBuilder();
        _dayOrders.ForEachAction(pair => result.AppendLine($"{pair.Key.InfoData.Name} {pair.Value}$"));
        return result.ToString();
    }

    public void ResetCurrentMoney() => CurrentMoney = TotalMoney;
}
