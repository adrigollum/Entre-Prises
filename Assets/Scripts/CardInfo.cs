using TMPro;
using UnityEngine;

public class CardInfo : MonoBehaviour
{
    public string cardName;
    public string cardDescription;
    public EnumCardType.CardType cardType;
    public int cardWattctionCost;

    public TextMeshPro cardNameText;
    public TextMeshPro cardDescriptionText;
    public TextMeshPro cardTypeText;
    public TextMeshPro cardWattctionCostText;

    private void Start()
    {
        cardNameText.text = cardName;
        cardDescriptionText.text = cardDescription;
        cardWattctionCostText.text = cardWattctionCost.ToString() + "W";
        cardTypeText.text = EnumCardType.TypeToString(cardType);
    }
}
