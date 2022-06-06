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

    [ReadOnly, SerializeField] private List<float> _coefficients;
    public IReadOnlyList<float> CoefficientsDictionary => _coefficients;

    public DrinksDescriptionCoefficients(float[] coefficients)
    {
        _coefficients = new List<float>();
        var i = 0;
        for (; i < coefficients.Length; ++i)
        {
            _coefficients.Add(coefficients[i]);
        }
        for(; i < 6; ++i)
        {
            _coefficients.Add(0.5f);
        }
    }

    public float GetAverageDifference(DrinksDescriptionCoefficients anotherCoefficients)
    {
        var average = 0f;
        for (var i = 0; i < _coefficients.Count; i++)
        {
            average += Mathf.Abs(
                _coefficients[i] - 
                anotherCoefficients.CoefficientsDictionary[i]
                );
        }

        average /= _coefficients.Count;
        Debug.Log(average);
        return average;
    }
}
