using UnityEngine;

public class EnemyTurn : MonoBehaviour
{
    public EnemyInfo enemyInfo;

    public void TakeDamage(CardInfo cardInfo)
    {
        if (cardInfo != null)
        {
            enemyInfo.TakeDamage(cardInfo);
        }
        else
        {
            Debug.LogError("CardInfo component not found on the card.");
        }
    }
}
