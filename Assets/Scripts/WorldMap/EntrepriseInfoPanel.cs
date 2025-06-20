using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class EntrepriseInfoPanel : MonoBehaviour
{
    public TextMeshProUGUI nomText;
    public TextMeshProUGUI niveauText;
    public TextMeshProUGUI etatText;

    public List<Button> boutonsForces;
    public List<Button> boutonsFaiblesses;

    private string nom;
    private int niveau;
    private EnterPriseState etat;

    // Ici on reçoit juste des noms, et on délègue à ForceFaiblesseItem la gestion de prix/achat
    public void Setup(string nom, int niveau, EnterPriseState etat, List<string> forces, List<string> faiblesses)
    {
        this.nom = nom;
        this.niveau = niveau;
        this.etat = etat;

        nomText.text = nom;
        niveauText.text = $"Niveau : {niveau}";
        // Cache tous les boutons d’abord
        foreach (var btn in boutonsForces) btn.gameObject.SetActive(false);
        foreach (var btn in boutonsFaiblesses) btn.gameObject.SetActive(false);

        // Active et initialise les boutons forces
        for (int i = 0; i < forces.Count && i < boutonsForces.Count; i++)
        {
            boutonsForces[i].gameObject.SetActive(true);

            ForceFaiblesseItem item = boutonsForces[i].GetComponent<ForceFaiblesseItem>();
            if (item != null)
                item.Initialiser(forces[i], true);
            else
                Debug.LogWarning("ForceFaiblesseItem manquant sur bouton force");
        }

        // Active et initialise les boutons faiblesses
        for (int i = 0; i < faiblesses.Count && i < boutonsFaiblesses.Count; i++)
        {
            boutonsFaiblesses[i].gameObject.SetActive(true);

            ForceFaiblesseItem item = boutonsFaiblesses[i].GetComponent<ForceFaiblesseItem>();
            if (item != null)
                item.Initialiser(faiblesses[i], false);
            else
                Debug.LogWarning("ForceFaiblesseItem manquant sur bouton faiblesse");
        }

        gameObject.SetActive(true);
    }

    public void OnClickLancerCombat()
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
}
