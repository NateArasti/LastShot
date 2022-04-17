using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class InkyParser
{
    public static PhraseData ParsePhrase(string line, IReadOnlyCollection<Character> participants)
    {
        var characterEndIndex = line.IndexOf(':');
        if (characterEndIndex == -1)
            throw new UnityException($"No character in line - {line}");

        var characterName = line[..characterEndIndex];
        var character = ParseCharacter(characterName, participants);

        var phrase = line.Substring(characterEndIndex + 1, line.Length - characterEndIndex - 1);
        var orderTag = Regex.Match(phrase, @"<.*>.*</.*>");
        var coloredPhrase = "";
        Drink drink = null;

        if (orderTag != Match.Empty)
        {
            var orderTagValue = orderTag.Value;
            var orderKeyName = Regex.Match(orderTagValue, @"_(.*?)>").Groups[1].Value;
            if (DatabaseManager.DrinkDatabase.TryGetValue(orderKeyName, out drink))
                coloredPhrase = phrase.Replace(orderTagValue, 
                    $"<color=#ffa500ff>{drink.InfoData.Name}</color>");
            else 
                throw new UnityException($"No such drink {orderKeyName}");
        }
        if (coloredPhrase == string.Empty)
            coloredPhrase = phrase;

        return new PhraseData(character, drink, coloredPhrase, phrase);
    }

    private static Character ParseCharacter(string characterName, IReadOnlyCollection<Character> participants)
    {
        var name = DatabaseManager.CharacterDatabase.TryGetName(characterName);
        if(! participants.TryGetObject(
               characterName, 
               (ch, s) => ch.KeyName == s,
               out var character))
        {
            throw new UnityException($"No character in line, parsed name - {characterName},{name}");
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