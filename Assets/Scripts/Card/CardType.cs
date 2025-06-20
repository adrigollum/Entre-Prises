using UnityEngine;

public class EnumCardType
{
    public enum CardType
    {
        None,
        Cafe,
        Gift,
        ChitChat,
        Bribe
    }

    public static string TypeToString(CardType type)
    {
        switch (type)
        {
            case CardType.Cafe:
                return "Café";
            case CardType.Gift:
                return "Cadeau";
            case CardType.ChitChat:
                return "Papotage";
            case CardType.Bribe:
                return "Pot-de-vin";
            default:
                return "";
        }
    }
    public static CardType StringToType(string type)
    {
        switch (type)
        {
            case "Café":
                return CardType.Cafe;
            case "Cadeau":
                return CardType.Gift;
            case "Papotage":
                return CardType.ChitChat;
            case "Pot-de-vin":
                return CardType.Bribe;
            default:
                return CardType.None;
        }
    }
}