using UnityEngine;

public class EnemyTurn : MonoBehaviour
{
    public EnemyInfo enemyInfo;

    public void Init()
    {
        enemyInfo.Init();
    }
    public int GetDamage(CardInfo cardInfo)
    {
        if (cardInfo == null)
        {
            Debug.LogError("CardInfo is null. Cannot calculate damage.");
            return 0;
        }

        return enemyInfo.GetDamage(cardInfo);
    }
}
