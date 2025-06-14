using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum EnterPriseState
{
    Neutral,
    Win,
    APortee,
    Perdue
}

[RequireComponent(typeof(Button))]
public class EntrepriseNode : MonoBehaviour
{
    public string nom;
    public int niveau;
    public EnterPriseState etat;
    public List<EntrepriseNode> connexions;

    private Image imageBouton;

    void Awake()
    {
        // Récupère automatiquement l’image du bouton
        imageBouton = GetComponent<Image>();

        etat = EnumGameStatusToEnterPriseState(
            StaticEntreprisesSaveManager.GetEnemyStatus(nom));

        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        if (etat == EnterPriseState.APortee)
        {
            StaticEnemyInfo.name = nom;
            StaticEnemyInfo.level = niveau;
            SceneManager.LoadScene("Combat");
        }
        else
        {
            Debug.LogWarning($"Le combat contre {nom} n'est pas disponible dans l'état actuel : {etat}");
        }
    }
    private EnterPriseState EnumGameStatusToEnterPriseState(EnumGameStatus gameStatus)
    {
        switch (gameStatus)
        {
            case EnumGameStatus.Won:
                return EnterPriseState.Win;
            case EnumGameStatus.Lost:
                return EnterPriseState.Perdue;
            default:
                return EnterPriseState.Neutral;
        }
    }

    public void UpdateColor()
    {
        if (imageBouton == null) return;

        switch (etat)
        {
            case EnterPriseState.Neutral:
                imageBouton.color = MapController.colorNeutral;
                break;
            case EnterPriseState.Win:
                imageBouton.color = MapController.colorWin;
                break;
            case EnterPriseState.APortee:
                imageBouton.color = MapController.colorNear;
                break;
            case EnterPriseState.Perdue:
                imageBouton.color = MapController.colorLost;
                break;
        }
    }
}
