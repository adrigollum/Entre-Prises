using UnityEngine;

public class Squich : MonoBehaviour
{
    private Vector3 originalScale;
    private bool isSquishing = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
