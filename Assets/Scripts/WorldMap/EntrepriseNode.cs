using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

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

    public EntrepriseInfoPanel infoPanel;

    public List<string> resistances = new List<string>();
    public List<string> weaknesses = new List<string>();

    private Image imageBouton;

    [SerializeField] private AudioClip ClicSound;

    [SerializeField] private AudioSource SFX;

    void Awake()
    {
        imageBouton = GetComponent<Image>();
        etat = EnumGameStatusToEnterPriseState(
            StaticEntreprisesSaveManager.GetEnemyStatus(nom));

        GetTypesFromSave();

        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void GetTypesFromSave()
    {
        weaknesses.Clear();
        resistances.Clear();

        Tuple<List<EnumCardType.CardType>, List<EnumCardType.CardType>> types = StaticEnemyInfo.GetSaveEnemyInfo(nom);

        if (types == null)
        {
            if (niveau == 1)
            {
                types = StaticEnemyInfo.GetRandomTypeList(1, 1);
            }
            else if (niveau == 2)
            {
                types = StaticEnemyInfo.GetRandomTypeList(2, 2);
            }
            else
            {
                types = StaticEnemyInfo.GetRandomTypeList(1, 2);
            }
        }

        if (types == null)
        {
            Debug.LogError("Failed to get types for enemy: " + nom);
            return;
        }
        StaticEnemyInfo.SaveEnemyInfo(nom, types.Item1, types.Item2);

        weaknesses = types.Item1.ConvertAll(type => EnumCardType.TypeToString(type));
        resistances = types.Item2.ConvertAll(type => EnumCardType.TypeToString(type));
    }

    public void OnClick()
    {
        if (infoPanel != null)
        {
            SFX.PlayOneShot(ClicSound);
            infoPanel.gameObject.SetActive(true); // Active le panel si désactivé
            infoPanel.Setup(nom, niveau, etat, weaknesses, resistances);
        }
        else
        {
            Debug.LogError("Aucun InfoPanel assigné à ce noeud !");
        }
    }

    private EnterPriseState EnumGameStatusToEnterPriseState(EnumGameStatus gameStatus)
    {
        return gameStatus switch
        {
            EnumGameStatus.Won => EnterPriseState.Win,
            EnumGameStatus.Lost => EnterPriseState.Perdue,
            _ => EnterPriseState.Neutral,
        };
    }

    public void UpdateColor()
    {
        if (imageBouton == null) return;

        imageBouton.color = etat switch
        {
            EnterPriseState.Neutral => MapController.colorNeutral,
            EnterPriseState.Win => MapController.colorWin,
            EnterPriseState.APortee => MapController.colorNear,
            EnterPriseState.Perdue => MapController.colorLost,
            _ => imageBouton.color
        };
    }
}
