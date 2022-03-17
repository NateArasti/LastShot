using System.Text.RegularExpressions;
using UnityEngine;

public static class InkyParser
{
    public static (Character character, Ingredient alcohol, string coloredPhrase, string simplePhrase) ParsePhrase(string line)
    {
        var characterEndIndex = line.IndexOf(':');
        if (characterEndIndex == -1)
            throw new UnityException($"No character in line - {line}");
        Ingredient alcohol = null;
        string phrase;
        if (DatabaseManager.CharacterDatabase.TryGetValue(line.Substring(0, characterEndIndex), out var character))
            phrase = line.Substring(characterEndIndex + 1, line.Length - characterEndIndex - 1);
        else
            throw new UnityException($"No character in line, parsed name - {line.Substring(0, characterEndIndex)}");

        var orderOpenTag = Regex.Match(phrase, @"<[^>]*>");
        var orderCloseTag = Regex.Match(phrase, @"</[^>]*>");
        var coloredPhrase = "";
        if (orderOpenTag != Match.Empty)
        {
            var alcoholKeyName = orderOpenTag.Value.Substring(5, orderOpenTag.Length - 6);
            if (DatabaseManager.AlcoholDatabase.TryGetValue(alcoholKeyName, out alcohol))
            {
                coloredPhrase = phrase.Replace(orderOpenTag.Value, "<color=#ffa500ff>");
                coloredPhrase = coloredPhrase.Replace(orderCloseTag.Value, "</color>");
            }
            orderOpenTag = Regex.Match(phrase, @"<[^>]*>");
            phrase = phrase.Remove(orderOpenTag.Index, orderOpenTag.Length);
            orderCloseTag = Regex.Match(phrase, @"</[^>]*>");
            phrase = phrase.Remove(orderCloseTag.Index, orderCloseTag.Length);
        }
        if (coloredPhrase == "")
            coloredPhrase = phrase;
        return (character, alcohol, coloredPhrase, phrase);
    }
}