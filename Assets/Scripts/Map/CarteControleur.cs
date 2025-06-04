using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CarteControleur : MonoBehaviour {
    public List<EntrepriseNode> toutesLesEntreprises;

    [Header("Affichage des lignes")]
    public Transform lignesParent;
    public GameObject lignePrefab; // Une simple image 1x1
    public float epaisseurLigne = 2f;

    [Header("Affichage des prises")]
    public Sprite spritePrise;
    public Vector2 taillePrise = new Vector2(30, 30);

    [Header("Couleurs par état")]
    public Color couleurGagnee = Color.green;
    public Color couleurAPortee = Color.yellow;
    public Color couleurNeutre = Color.gray;
    public Color couleurPerdue = Color.red;

    private List<LigneUI> lignes = new List<LigneUI>();
    private List<PriseUI> prises = new List<PriseUI>();

    void Start() {
        
        CreerLignesEntreprises();
        CreerPrises();
    }

    void Update() {
        MettreAJourEtats();
        MettreAJourLignes();
        MettreAJourPrises();
    }

    void MettreAJourEtats() {
        foreach (EntrepriseNode node in toutesLesEntreprises) {
            if (node.etat == EtatEntreprise.Gagnee) {
                foreach (EntrepriseNode conn in node.connexions) {
                    if (conn.etat == EtatEntreprise.Neutre) {
                        conn.etat = EtatEntreprise.APortee;
                    }
                }
            }
        }

        foreach (EntrepriseNode node in toutesLesEntreprises) {
            node.MettreAJourCouleur();
        }
    }

    void CreerLignesEntreprises() {
        HashSet<string> lignesCreees = new HashSet<string>();

        foreach (EntrepriseNode a in toutesLesEntreprises) {
            foreach (EntrepriseNode b in a.connexions) {
                string id1 = a.name + "_" + b.name;
                string id2 = b.name + "_" + a.name;

                if (!lignesCreees.Contains(id1) && !lignesCreees.Contains(id2)) {
                    lignesCreees.Add(id1);

                    GameObject ligneGO = Instantiate(lignePrefab, lignesParent);
                    RectTransform ligneRT = ligneGO.GetComponent<RectTransform>();

                    lignes.Add(new LigneUI {
                        a = a.GetComponent<RectTransform>(),
                        b = b.GetComponent<RectTransform>(),
                        imageRT = ligneRT
                    });
                }
            }
        }
    }

    void CreerPrises() {
        foreach (EntrepriseNode node in toutesLesEntreprises) {
            GameObject priseGO = new GameObject("Prise_" + node.name);
            priseGO.transform.SetParent(lignesParent, false);

            Image image = priseGO.AddComponent<Image>();
            image.sprite = spritePrise;
            image.raycastTarget = false;

            RectTransform rt = image.rectTransform;
            rt.sizeDelta = taillePrise;
            rt.pivot = new Vector2(0.5f, 0.5f);

            prises.Add(new PriseUI {
                node = node,
                image = image,
                imageRT = rt
            });
        }
    }

    void MettreAJourLignes() {
    foreach (LigneUI ligne in lignes) {
        Vector3 posA = GetCentreMonde(ligne.a);
        Vector3 posB = GetCentreMonde(ligne.b);

        Vector3 direction = posB - posA;
        float longueur = direction.magnitude;

        ligne.imageRT.pivot = new Vector2(0, 0.5f);
        ligne.imageRT.position = posA;
        ligne.imageRT.sizeDelta = new Vector2(longueur, epaisseurLigne);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        ligne.imageRT.rotation = Quaternion.Euler(0, 0, angle);

        // 🎨 Coloration dynamique
        EtatEntreprise etatA = ligne.a.GetComponent<EntrepriseNode>().etat;
        EtatEntreprise etatB = ligne.b.GetComponent<EntrepriseNode>().etat;

        Color couleurLigne;

        if (etatA == EtatEntreprise.Perdue || etatB == EtatEntreprise.Perdue ) {
    couleurLigne = couleurPerdue; // 🔴
}
else if (etatA == EtatEntreprise.Gagnee && etatB == EtatEntreprise.Gagnee) {
    couleurLigne = couleurGagnee; // 🟢
}
else if ((etatA == EtatEntreprise.Gagnee || etatB == EtatEntreprise.Gagnee ) &&
         (etatA == EtatEntreprise.APortee || etatB == EtatEntreprise.APortee)) {
    couleurLigne = couleurAPortee; // 🟡
}
else {
    couleurLigne = couleurNeutre; // ⚪
}


        ligne.imageRT.GetComponent<Image>().color = couleurLigne;
    }
}


    void MettreAJourPrises() {
        foreach (PriseUI prise in prises) {
            Vector3 position = GetCentreMonde(prise.node.GetComponent<RectTransform>());
            prise.imageRT.position = position;

            // Met à jour la couleur selon l’état
            switch (prise.node.etat) {
                case EtatEntreprise.Gagnee:
                    prise.image.color = couleurGagnee;
                    break;
                case EtatEntreprise.APortee:
                    prise.image.color = couleurAPortee;
                    break;
                case EtatEntreprise.Neutre:
                    prise.image.color = couleurNeutre;
                    break;
                case EtatEntreprise.Perdue:
                    prise.image.color = couleurPerdue;
                    break;
            }
        }
    }

    Vector3 GetCentreMonde(RectTransform rt) {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        return (corners[0] + corners[2]) / 2f;
    }

    class LigneUI {
        public RectTransform a;
        public RectTransform b;
        public RectTransform imageRT;
    }

    class PriseUI {
        public EntrepriseNode node;
        public Image image;
        public RectTransform imageRT;
    }
}
