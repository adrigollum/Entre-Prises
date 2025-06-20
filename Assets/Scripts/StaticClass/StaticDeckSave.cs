
using System.Collections.Generic;
using UnityEngine;

public static class StaticDeckSave
{
    private const string DEFAULT_DECK = "Boite de Chocowat:5|Cable News:5|Dipoleboire:10|Espresso:5|Louis Bobine:5|Piezo Quatre Fromage:5|Ristretto:5";
    private const string DEFAULT_NOT_DECK = "Album de Joule:4|Boite de Chocowat:5|Cable News:1|Café Noir:3|Circuit Court et Vice et Versa:3|Cohmerage:2|Corrupt'Ion:2|Dipoleboire:1|Dithirampere:2|Doppio:4|Espresso:1|Fusible de Chasse:3|Graissage de Pwhatt:3|Invitation au jeux Ohmlympique:2|Le Loup de VoltStreet:4|Louis Bobine:5|Lungo:2|Piezo Quatre Fromage:1|Pot de Kelvin:5|Prime Electrique:4|Ristretto:5|Supraconductivichy:2";

    public enum DeckType
    {
        Deck,
        NotDeck
    }

    private static string DictionaryToString(Dictionary<string, int> dictionary)
    {
        // key1:value1|key2:value2|...
        List<string> entries = new List<string>();
        foreach (var kvp in dictionary)
        {
            entries.Add($"{kvp.Key}:{kvp.Value}");
        }
        return string.Join("|", entries);
    }

    private static Dictionary<string, int> StringToDictionary(string str)
    {
        // key1:value1|key2:value2|...
        Dictionary<string, int> dictionary = new Dictionary<string, int>();
        string[] entries = str.Split('|');
        foreach (string entry in entries)
        {
            string[] kvp = entry.Split(':');
            if (kvp.Length == 2 && int.TryParse(kvp[1], out int value))
            {
                dictionary[kvp[0]] = value;
            }
            else
            {
                Debug.LogWarning($"Invalid entry in deck string: {entry}");
            }
        }
        return dictionary;
    }

    public static string DeckTypeToString(DeckType deckType)
    {
        return deckType switch
        {
            DeckType.Deck => "Deck",
            DeckType.NotDeck => "NotDeck",
            _ => "Unknown"
        };
    }

    public static void SaveDeck(DeckType deckName, Dictionary<string, int> deckCards)
    {
        string deckTypeString = DeckTypeToString(deckName);
        // Save in PlayerPrefs
        string deckString = DictionaryToString(deckCards);
        PlayerPrefs.SetString(deckTypeString, deckString);
        PlayerPrefs.Save();
        Debug.Log($"Deck saved: {deckTypeString} - {deckString}");
    }

    public static Dictionary<string, int> GetDeck(DeckType deckName)
    {
        string deckTypeString = DeckTypeToString(deckName);
        if (PlayerPrefs.HasKey(deckTypeString))
        {
            string deckString = PlayerPrefs.GetString(deckTypeString);
            return StringToDictionary(deckString);
        }
        else
        {
            // Init first save
            if (deckName == DeckType.Deck)
            {
                SaveDeck(deckName, StringToDictionary(DEFAULT_DECK));
                return StringToDictionary(DEFAULT_DECK);
            }
            else if (deckName == DeckType.NotDeck)
            {
                SaveDeck(deckName, StringToDictionary(DEFAULT_NOT_DECK));
                return StringToDictionary(DEFAULT_NOT_DECK);
            }
            else
            {
                Debug.LogWarning($"No deck found for {deckTypeString}. Returning empty dictionary.");
                return new Dictionary<string, int>();
            }
        }
    }
}
