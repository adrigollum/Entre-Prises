using System.Globalization;
using TMPro;
using UnityEngine;

public class CardInfo : MonoBehaviour
{
    public int MaxCardCount = 10; // This number should not be dynamically changed, but set in the inspector.

    public string cardName;
    public string cardDescription;
    public EnumCardType.CardType cardType;
    public int cardWattctionCost;
    public int cardStat;

    public TextMeshPro cardNameText;
    public TextMeshPro cardDescriptionText;
    public TextMeshPro cardTypeText;
    public TextMeshPro cardWattctionCostText;
    public TextMeshPro cardStatText;

    private void Start()
    {
        cardNameText.text = cardName;
        cardDescriptionText.text = cardDescription;
        cardWattctionCostText.text = cardWattctionCost.ToString() + "W";
        cardTypeText.text = EnumCardType.TypeToString(cardType);
        cardStatText.text = cardStat.ToString(CultureInfo.InvariantCulture) + "%";
    }
}
