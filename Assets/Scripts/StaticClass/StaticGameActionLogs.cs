
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static class StaticGameActionLogs
{
    public enum EnumCardEffect
    {
        Neutral,
        Weak,
        Resistant,
    }
    public static Color NeutralColor = Color.blue;
    public static Color WeakColor = Color.green;
    public static Color ResistantColor = Color.red;
    public static Color StartTurnColor = Color.yellow;
    public static Color DiscardColor = Color.lightGreen;
    public static Color SkipTurnColor = Color.gray;
    public static Color EnemyAttackColor = Color.magenta;

    public static List<string> gameActionLogs = new List<string>();
    public const int MAX_LOGS = 6;
    public static void AddLog(string log)
    {
        if (gameActionLogs.Count >= MAX_LOGS)
        {
            gameActionLogs.RemoveAt(0); // Remove the oldest log
        }

        gameActionLogs.Add(log);
    }

    private static string ColorToHex(Color color)
    {
        return $"#{ColorUtility.ToHtmlStringRGB(color)}";
    }
    public static string GetLogs()
    {
        return string.Join("\n", gameActionLogs);
    }
    public static void AddPlayingCardLog(int stat, EnumCardType.CardType cardType, EnumCardEffect effect)
    {
        string cardTypeName = EnumCardType.TypeToString(cardType);
        string action = effect switch
        {
            EnumCardEffect.Neutral => $"<color={ColorToHex(NeutralColor)}>Gagne {stat}% car l'adversaire est neutre face à {cardTypeName}</color>",
            EnumCardEffect.Weak => $"<color={ColorToHex(WeakColor)}>Gagne {stat}% car l'adversaire est faible face à {cardTypeName}</color>",
            EnumCardEffect.Resistant => $"<color={ColorToHex(ResistantColor)}>Perdu {stat}% car l'adversaire est résistant face à {cardTypeName}</color>",
            _ => "Action inconnue"
        };
        AddLog(action);
    }

    public static void AddTurnStartLog(int wattction)
    {
        string action = $"<color={ColorToHex(StartTurnColor)}>Début de tour, vous avez gagné {wattction} Wattctions</color>";
        AddLog(action);
    }

    public static void AddDiscardCardLog(int wattction)
    {
        string action = $"<color={ColorToHex(DiscardColor)}>Vous avez défaussé une carte, vous avez gagné {wattction} Wattctions</color>";
        AddLog(action);
    }

    public static void AddSkipTurnLog(int wattction)
    {
        string action = $"<color={ColorToHex(SkipTurnColor)}>Vous avez passé votre tour, vous avez gagné {wattction} Wattctions</color>";
        AddLog(action);
    }

    public static void AddEnemyAttackLog(int damage)
    {
        string action = $"<color={ColorToHex(EnemyAttackColor)}>L'ennemi vous attaque et vous inflige {damage} dégâts</color>";
        AddLog(action);
    }
}
