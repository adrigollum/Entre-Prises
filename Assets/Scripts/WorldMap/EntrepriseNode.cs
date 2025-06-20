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

    public EntrepriseInfoPanel infoPanel;

    public List<string> forces = new();
    public List<string> faiblesses = new();

    private Image imageBouton;

    void Awake()
    {
        imageBouton = GetComponent<Image>();
        etat = EnumGameStatusToEnterPriseState(
            StaticEntreprisesSaveManager.GetEnemyStatus(nom));

        GenererForcesFaiblesses();

        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void GenererForcesFaiblesses()
{
    if (forces.Count > 0 || faiblesses.Count > 0) return;

    System.Random rand = new();

    // 4 paires exclusives
    List<(string force, string faiblesse)> pairesDisponibles = new()
    {
        ("Allergique au café", "Dépendant du café"),
        ("Capitellophobe", "Imbu de sa personne"),
        ("Introvertie", "Pipelette"),
        ("Intègre", "Radin")
    };

    int nbForces = 0;
    int nbFaiblesses = 0;

    switch (niveau)
    {
        case 1:
            nbForces = 1;
            nbFaiblesses = 1;
            break;
        case 2:
            nbForces = 2;
            nbFaiblesses = 2;
            break;
        case 3:
            nbForces = 2;
            nbFaiblesses = 1;
            break;
    }

    // Shuffle des paires
    List<(string force, string faiblesse)> shuffled = new(pairesDisponibles);
    for (int i = 0; i < shuffled.Count; i++)
    {
        int j = rand.Next(i, shuffled.Count);
        (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
    }

    int totalATirer = nbForces + nbFaiblesses;
    for (int i = 0; i < totalATirer && i < shuffled.Count; i++)
    {
        var paire = shuffled[i];

        // Tirer d'abord les faiblesses, puis les forces
        if (nbFaiblesses > 0)
        {
            faiblesses.Add(paire.faiblesse);
            nbFaiblesses--;
        }
        else if (nbForces > 0)
        {
            forces.Add(paire.force);
            nbForces--;
        }
    }
}


    public void OnClick()
    {
        if (infoPanel != null)
        {
            infoPanel.gameObject.SetActive(true); // Active le panel si désactivé
            infoPanel.Setup(nom, niveau, etat, forces, faiblesses);
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
