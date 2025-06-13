using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum EtatEntreprise {
    Neutre,
    Gagnee,
    APortee,
    Perdue
}

[RequireComponent(typeof(Button))]
public class EntrepriseNode : MonoBehaviour {
    public string nom;
    public int niveau;
    public EtatEntreprise etat;
    public List<EntrepriseNode> connexions;

    // couleurs définies dans l’inspecteur
    public Color couleurNeutre = Color.gray;
    public Color couleurGagnee = Color.green;
    public Color couleurAPortee = Color.red;
    public Color couleurPerdue = Color.black;

    private Image imageBouton;

    void Awake() {
        // Récupère automatiquement l’image du bouton
        imageBouton = GetComponent<Image>();
        if (imageBouton == null) {
            Debug.LogError($"{name} n’a pas de composant Image sur le même objet que le Button !");
        }
    }
    
    

    public void MettreAJourCouleur()
    {
        if (imageBouton == null) return;

        switch (etat)
        {
            case EtatEntreprise.Neutre:
                imageBouton.color = couleurNeutre;
                break;
            case EtatEntreprise.Gagnee:
                imageBouton.color = couleurGagnee;
                break;
            case EtatEntreprise.APortee:
                imageBouton.color = couleurAPortee;
                break;
            case EtatEntreprise.Perdue:
                imageBouton.color = couleurPerdue;
                break;
        }
    }
}
