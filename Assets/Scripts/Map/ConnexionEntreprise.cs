using UnityEngine;

public class ConnexionEntreprise : MonoBehaviour {
    public RectTransform pointA;
    public RectTransform pointB;

    private LineRenderer line;

    void Start() {
        line = gameObject.AddComponent<LineRenderer>();
        line.positionCount = 2;
        line.widthMultiplier = 2f;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = Color.white;
        line.endColor = Color.white;
        line.sortingOrder = 10;
    }

    void Update() {
        Vector3 posA = pointA.position;
        Vector3 posB = pointB.position;
        line.SetPosition(0, posA);
        line.SetPosition(1, posB);
    }
}
