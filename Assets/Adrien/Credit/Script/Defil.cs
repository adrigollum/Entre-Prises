using UnityEngine;

public class Defil : MonoBehaviour
{
    public float vitesseDefilement = 30f;

    void Update()
    {
        // On déplace l'objet vers le haut à la vitesse définie
        transform.Translate(Vector3.up * vitesseDefilement * Time.deltaTime);
    }
}
