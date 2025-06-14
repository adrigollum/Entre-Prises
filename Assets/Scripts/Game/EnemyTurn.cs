using UnityEngine;

public class EnemyTurn : MonoBehaviour
{
    public EnemyInfo enemyInfo;

    public void Init()
    {
        enemyInfo = GetComponent<EnemyInfo>();
        enemyInfo.Init();
    }
    public int GetDamageFromCard(CardInfo cardInfo)
    {
        if (cardInfo == null)
        {
            Debug.LogError("CardInfo is null. Cannot calculate damage.");
            return 0;
        }

        return enemyInfo.GetDamageFromCard(cardInfo);
    }

    public int GetAttackDamage()
    {
        return enemyInfo.GetAttackDamage();
    }

    public void Save(EnumGameStatus gameStatus)
    {
        enemyInfo.Save(gameStatus);
    }
}
