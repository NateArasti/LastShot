using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class InkyParser
{
    public static PhraseData ParsePhrase(string line, IReadOnlyCollection<Character> participants, Drink overrideDrink = null)
    {
        var characterEndIndex = line.IndexOf(':');
        if (characterEndIndex == -1)
            throw new UnityException($"No character in line - {line}");

        var characterName = line[..characterEndIndex];
        var character = ParseCharacter(characterName, participants);

        var phrase = line.Substring(characterEndIndex + 1, line.Length - characterEndIndex - 1);
        var orderTag = Regex.Match(phrase, @"<.*>(.*)</.*>");
        var coloredPhrase = "";
        var simplePhrase = "";
        Drink drink = null;

        if (orderTag != Match.Empty)
        {
            var orderTagValue = orderTag.Value;
            var orderText = orderTag.Groups[1].Value;
            var orderKeyName = Regex.Match(orderTagValue, @"_(.*?)>").Groups[1].Value;
            Debug.Log(orderKeyName);
            if (DatabaseManager.DrinkDatabase.TryGetValue(orderKeyName, out drink))
            {
                if (orderKeyName == DatabaseManager.DrinkDatabase.AnythingKeyName && overrideDrink != null)
                    orderText = overrideDrink.InfoData.Name.ToLower();
                simplePhrase = phrase.Clone().ToString().Replace(orderTagValue, orderText);
                coloredPhrase = phrase.Clone().ToString().Replace(orderTagValue,
                    $"<color=#ffa500ff>{orderText}</color>");
            }
            else 
                throw new UnityException($"No such drink {orderKeyName}");
        }

        if (simplePhrase == string.Empty)
        {
            simplePhrase = coloredPhrase = phrase;
        }

        return new PhraseData(character, drink, coloredPhrase, simplePhrase);
    }

    private static Character ParseCharacter(string characterName, IReadOnlyCollection<Character> participants)
    {
        var name = DatabaseManager.CharacterDatabase.TryGetName(characterName);
        if(! participants.TryGetObject(
               characterName, 
               (ch, s) => ch.DialogueKeyName == s,
               out var character))
        {
            throw new UnityException($"No character in line, parsed name - {characterName}, {name}");
        }

        character.CharacterName = name;

        return character;
    }

    public readonly struct PhraseData
    {
        public readonly Character PhraseCharacter;
        public readonly Drink Drink;
        public readonly string ColoredPhrase;
        public readonly string SimplePhrase;

        public PhraseData(Character character, Drink alcohol, string coloredPhrase, string simplePhrase)
        {
            PhraseCharacter = character;
            Drink = alcohol;
            ColoredPhrase = coloredPhrase;
            SimplePhrase = simplePhrase;
        }
    }
}