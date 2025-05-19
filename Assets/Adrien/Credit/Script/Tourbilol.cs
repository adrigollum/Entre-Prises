using UnityEngine;

public class Tourbilol : MonoBehaviour
{
   
    public float vitesseRotation = 90f;

    void Update()
    {
        // Rotation autour de l'axe Y en fonction de la vitesse (positive ou négative)
        transform.Rotate(0f, vitesseRotation * Time.deltaTime, 0f);
    }
}
