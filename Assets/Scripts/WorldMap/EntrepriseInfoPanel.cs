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

    public Button btnStartCombat;  // Bouton Start à lier dans l’inspecteur

    private string nom;
    private int niveau;
    private EnterPriseState etat;

    public void Setup(string nom, int niveau, EnterPriseState etat, List<string> weaknesses, List<string> resistances)
    {
        this.nom = nom;
        this.niveau = niveau;
        this.etat = etat;

        nomText.text = nom;
        niveauText.text = $"Niveau : {niveau}";

        // Gérer l'affichage du texte état + activation bouton selon état
        switch (etat)
        {
            case EnterPriseState.Perdue:
                etatText.text = "Perdu";
                break;

            case EnterPriseState.Win:
                etatText.text = "Gagné";
                break;

            case EnterPriseState.APortee:
                etatText.text = "LetsGO";
                break;

            default:
                etatText.text = "Loin";
                break;
        }

        // Cache tous les boutons d’abord
        foreach (var btn in boutonsForces) btn.gameObject.SetActive(false);
        foreach (var btn in boutonsFaiblesses) btn.gameObject.SetActive(false);

        // Active et initialise les boutons forces
        for (int i = 0; i < resistances.Count && i < boutonsForces.Count; i++)
        {
            boutonsForces[i].gameObject.SetActive(true);

            ForceFaiblesseItem item = boutonsForces[i].GetComponent<ForceFaiblesseItem>();
            if (item != null)
                item.Initialiser(nom, resistances[i], true);
            else
                Debug.LogWarning("ForceFaiblesseItem manquant sur bouton force");
        }

        // Active et initialise les boutons faiblesses
        for (int i = 0; i < weaknesses.Count && i < boutonsFaiblesses.Count; i++)
        {
            boutonsFaiblesses[i].gameObject.SetActive(true);

            ForceFaiblesseItem item = boutonsFaiblesses[i].GetComponent<ForceFaiblesseItem>();
            if (item != null)
                item.Initialiser(nom, weaknesses[i], false);
            else
                Debug.LogWarning("ForceFaiblesseItem manquant sur bouton faiblesse");
        }

        gameObject.SetActive(true);
    }

    void Start()
    {
        // Assurez-vous que le bouton de combat est désactivé par défaut
        if (btnStartCombat != null)
        {
            btnStartCombat.onClick.AddListener(OnClickLancerCombat);
        }
        else
        {
            Debug.LogError("btnStartCombat n'est pas assigné dans l'inspecteur !");
        }
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
