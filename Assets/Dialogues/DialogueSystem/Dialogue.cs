using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Dialogue 0", menuName = "Characters/Dialogue")]
public class Dialogue : ScriptableObject
{
    [SerializeField] private TextAsset _inkyDialogue;
    [SerializeField] private Character[] _participants;

    public OrderData CurrentOrderData;

    public readonly UnityEvent OnEnd = new();
    public TextAsset Text => _inkyDialogue;
    public IReadOnlyCollection<Character> Participants => _participants;

    public struct OrderData
    {
        public Character Character;
        public Drink Drink;
        public CharacterGuestGrade Grade;

        public OrderData(Character character, Drink drink, CharacterGuestGrade grade = CharacterGuestGrade.Good)
        {
            Character = character;
            Drink = drink;
            Grade = grade;
        }
    }
}
