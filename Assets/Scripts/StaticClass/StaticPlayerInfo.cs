using System.Collections.Generic;

public static class StaticPlayerInfo
{
    public static Dictionary<int, int> levelToExp = new Dictionary<int, int>
    {
        { 1, 50 },
        { 2, 100 },
        { 3, 350 },
        { 4, 600 },
        { 5, int.MaxValue } // Level 5 has no upper limit
    };

    public static int ExpToLevel(int exp)
    {
        if (exp < levelToExp[1])
        {
            return 1;
        }
        else if (exp < levelToExp[1] + levelToExp[2])
        {
            return 2;
        }
        else if (exp < levelToExp[1] + levelToExp[2] + levelToExp[3])
        {
            return 3;
        }
        else if (exp < levelToExp[1] + levelToExp[2] + levelToExp[3] + levelToExp[4])
        {
            return 4;
        }
        else
        {
            return 5; // Level 5 or above
        }
    }

    public static int GetNextXpStepForUI(int exp)
    {
        int level = ExpToLevel(exp);
        if (level < 5)
        {
            return levelToExp[level];
        }
        else
        {
            return int.MaxValue; // No next step for level 5
        }
    }

    public static int ClampExp(int exp)
    {
        int i = 0;
        while (i < levelToExp.Count && exp >= levelToExp[i + 1])
        {
            exp -= levelToExp[i + 1];
            i++;
        }

        return exp;
    }
}
