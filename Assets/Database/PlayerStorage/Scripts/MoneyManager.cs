using System;
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
        _totalMoney = rand.Next(100, 500);
        _gainedOnCurrentDay = rand.Next(30, 100);
    }

    [SerializeField] private int _totalMoney;
    [SerializeField] private int _gainedOnCurrentDay;

    public int TotalMoney => _totalMoney;
    public int GainedOnCurrentDay => _gainedOnCurrentDay;
}
