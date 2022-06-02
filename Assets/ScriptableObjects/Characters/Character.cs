using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleCharacter", menuName = "Characters/Character")]
public class Character : ScriptableObject, IDatabaseObject
{
    [SerializeField] private string _keyName;
    [SerializeField] private Sprite _portrait;
    [SerializeField] private Guest _prefab;
    [SerializeField] private DrinksDescriptionCoefficients _coefficients;

    private bool _nameWritten;
    private Dictionary<string, float> _coefficientsDictionary;

    public string CharacterName { get; set; }
    public Sprite Portrait => _portrait;
    public string KeyName => _keyName;
    public Guest Prefab => _prefab;

    public CharacterGuestGrade GetCharacterGrade()
    {
        return CharacterGuestGrade.Excellent;
    }

    public void WriteData(string[] paramsLine)
    {
        var coefficients = new List<float>();
        for (var i = 1; i < paramsLine.Length; ++i)
        {
            coefficients.Add(float.Parse(paramsLine[i]));
        }
        _coefficients = new DrinksDescriptionCoefficients(coefficients.ToArray());
    }
}
