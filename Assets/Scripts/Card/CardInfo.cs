using System.Globalization;
using TMPro;
using UnityEngine;

public class CardInfo : MonoBehaviour
{
    public int MaxCardInDeck = 10;


    private int id;
    public int Id
    {
        get { return id; }
    }

    public string cardName;
    public string cardDescription;
    public EnumCardType.CardType cardFirstType;
    public EnumCardType.CardType cardSecondType;
    public int cardWattctionCost;
    public int cardStat;

    public TextMeshPro cardNameText;
    public TextMeshPro cardDescriptionText;
    public TextMeshPro cardFirstTypeText;
    public TextMeshPro cardSecondTypeText;
    public TextMeshPro cardWattctionCostText;
    public TextMeshPro cardStatText;

    private void Start()
    {
        cardNameText.text = cardName;
        cardDescriptionText.text = cardDescription;
        cardWattctionCostText.text = cardWattctionCost.ToString() + "W";
        cardFirstTypeText.text = EnumCardType.TypeToString(cardFirstType);
        cardSecondTypeText.text = EnumCardType.TypeToString(cardSecondType);
        cardStatText.text = cardStat.ToString(CultureInfo.InvariantCulture) + "%";
    }
}
