using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ForceFaiblesseItem : MonoBehaviour
{
    public TextMeshProUGUI nomTMP;
    public TextMeshProUGUI prixTMP;
    public Button bouton;

    public string nom;
    private bool estForce;
    private bool estAchete = false;
    private int prix = 100; // Exemple, tu peux adapter par force/faiblesse

    // Initialisation appelée par le panel avec le nom et si c'est force ou faiblesse
    public void Initialiser(string nom, bool estForce)
    {
        this.nom = nom;
        this.estForce = estForce;

        // Ici tu peux définir un prix selon la force/faiblesse (exemple simple)
        prix = estForce ? 100 : 50;

        MettreAJourAffichage();

        // Ajouter le listener sur le bouton pour achat
        bouton.onClick.RemoveAllListeners();
        bouton.onClick.AddListener(() => Acheter());
    }

    private void MettreAJourAffichage()
    {
        if (estAchete)
        {
            nomTMP.text = nom;
            prixTMP.text = "Acquis";
            prixTMP.color = Color.green;
        }
        else
        {
            // Affiche juste "Force" ou "Faiblesse" sans le prix ni le nom
            nomTMP.text = estForce ? "Force" : "Faiblesse";
            prixTMP.text = $"{prix}"; // ou tu peux laisser vide
            prixTMP.color = Color.red;
        }
    }

    // Méthode pour simuler l'achat
    public void Acheter()
    {
        if (!estAchete)
        {
            // Ici tu peux rajouter la logique de coût / ressources, etc.
            estAchete = true;
            MettreAJourAffichage();
            Debug.Log($"Tu as acheté : {nom}");
        }
    }
}
