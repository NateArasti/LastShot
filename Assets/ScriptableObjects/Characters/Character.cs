using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleCharacter", menuName = "Characters/Character")]
public class Character : ScriptableObject, IDatabaseObject
{
    public const float CoefficientsAcceptThreshold = 0.25f;

    [SerializeField] private string _keyName;
    [SerializeField] private string _dialogueKeyName;
    [SerializeField] private Sprite _portrait;
    [SerializeField] private Guest _prefab;
    [SerializeField] private DrinksDescriptionCoefficients _coefficients;

    public string CharacterName { get; set; }
    public Sprite Portrait => _portrait;
    public string KeyName => _keyName;
    public string DialogueKeyName
    {
        get => _dialogueKeyName;
        set => _dialogueKeyName = value;
    }

    public Guest Prefab => _prefab;

    public DrinksDescriptionCoefficients Coefficients => _coefficients;


    public CharacterGuestGrade GetCharacterGrade(Drink drink, IReadOnlyList<OrderAction> orderActions)
    {
        var comparison = OrderAction.Compare(drink.DrinkReceipt.PerfectActions, orderActions);
        Debug.Log(comparison);
        return comparison switch
        {
            > 0.6f => CharacterGuestGrade.Excellent,
            > 0.3f => CharacterGuestGrade.Good,
            _ => CharacterGuestGrade.Bad
        };
    }

    public bool CheckSuggestedDrink(Drink drink) => 
        drink.Coefficients.GetAverageDifference(Coefficients) < CoefficientsAcceptThreshold;

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
