using TMPro;
using UnityEngine;

public class MenuMoney : MonoBehaviour
{
    public static MenuMoney Instance { get; private set; } = null;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateUI();
    }

    public static bool AddMoney(int amount)
    {
        int currentMoney = PlayerPrefs.GetInt("Money", 0);
        if (currentMoney + amount < 0)
        {
            return false;
        }

        currentMoney += amount;

        PlayerPrefs.SetInt("Money", currentMoney);
        PlayerPrefs.Save();

        if (Instance != null)
        {
            Instance.UpdateUI();
        }

        return true;
    }

    public void UpdateUI()
    {
        int money = PlayerPrefs.GetInt("Money", 0);
        TextMeshProUGUI moneyText = GetComponent<TextMeshProUGUI>();
        moneyText.text = money.ToString() + " $";
    }
}
