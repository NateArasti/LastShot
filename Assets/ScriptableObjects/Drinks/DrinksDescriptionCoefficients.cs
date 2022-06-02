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

    [ReadOnly, SerializeField, TextArea(5, 10)] private string _coefsString;

    private readonly Dictionary<Coefficients, float> _coefficients;
    public IReadOnlyDictionary<Coefficients, float> CoefficientsDictionary => _coefficients;

    public DrinksDescriptionCoefficients(float[] coefficients)
    {
        _coefsString = "";
        _coefficients = new Dictionary<Coefficients, float>();
        var i = 0;
        for (; i < coefficients.Length; ++i)
        {
            var coefName = (Coefficients)i;
            _coefficients.Add(coefName, coefficients[i]);
            _coefsString += $"{coefName}: {_coefficients[coefName]}\n";
        }
        for(; i < 6; ++i)
        {
            var coefName = (Coefficients)i;
            _coefficients.Add(coefName, 0.5f);
            _coefsString += $"{coefName}: {_coefficients[coefName]}\n";
        }
        Debug.Log($"Successfully created coefficients:\n {_coefsString}");
    }

    public float GetAverageDifference(DrinksDescriptionCoefficients anotherCoefficients)
    {
        var average = 0f;
        for (var i = 0; i < _coefficients.Count; i++)
        {
            average += Mathf.Abs(
                _coefficients[(Coefficients) i] - 
                anotherCoefficients.CoefficientsDictionary[(Coefficients) i]
                );
        }

        average /= _coefficients.Count;
        Debug.Log(average);
        return average;
    }
}
