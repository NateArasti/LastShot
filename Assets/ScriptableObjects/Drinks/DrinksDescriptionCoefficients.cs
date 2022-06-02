using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DrinksDescriptionCoefficients
{
    public enum Coefficients
    {
        Strong,
        Bitter,
        Sour,
        Sweet,
        Spicy,
        Citrus
    }

    [ReadOnly, SerializeField, TextArea(5, 10)] private string coefsString = "";

    private readonly Dictionary<Coefficients, float> _cofficients;
    public IReadOnlyDictionary<Coefficients, float> Cofficients => _cofficients;

    public DrinksDescriptionCoefficients(float[] cofficients)
    {
        coefsString = "";
        _cofficients = new Dictionary<Coefficients, float>();
        var i = 0;
        for (; i < cofficients.Length; ++i)
        {
            var coefName = (Coefficients)i;
            _cofficients.Add(coefName, cofficients[i]);
            coefsString += $"{coefName}: {_cofficients[coefName]}\n";
        }
        for(; i < 6; ++i)
        {
            var coefName = (Coefficients)i;
            _cofficients.Add(coefName, 0.5f);
            coefsString += $"{coefName}: {_cofficients[coefName]}\n";
        }
        Debug.Log($"Successfully created coefficients:\n {coefsString}");
    }

    public float GetAverageDifference(DrinksDescriptionCoefficients anotherCoefficients)
    {
        return 0;
    }
}
