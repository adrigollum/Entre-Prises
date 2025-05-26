using UnityEngine;
using UnityEngine.InputSystem;

public class CamInteract : MonoBehaviour
{
    private Camera mainCamera;
    private PlayerTurn playerTurn;
    public GameObject cardSelected;

    public float minCardDistance = 1.5f;
    public float maxCardDistance = 3f;
    public float cardDistance = 2f;

    void Start()
    {
        mainCamera = GetComponent<Camera>();

        GameObject gameManager = GameObject.Find("GameManager");
        playerTurn = gameManager.GetComponent<PlayerTurn>();
    }

    void Update()
    {
        if (Mouse.current != null)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            Ray ray = mainCamera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                CardClick cardClick = hit.collider.GetComponent<CardClick>();
                if (cardClick != null && Mouse.current.leftButton.wasPressedThisFrame)
                {
                    if (cardSelected == null)
                    {
                        cardSelected = hit.collider.gameObject;
                    }
                    else
                    {
                        if (cardSelected.GetComponent<CardMovement>().isInPlayingArea)
                        {
                            playerTurn.PlayCard(cardSelected);
                        }
                        playerTurn.playerInventory.RepositionAllCards();
                        cardSelected = null;
                    }
                }
            }

            if (cardSelected != null)
            {
                float procCardDistance = cardDistance * mousePosition.y / Screen.height;
                procCardDistance = Mathf.Clamp(procCardDistance, minCardDistance, maxCardDistance);
                Vector3 newPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, procCardDistance));
                cardSelected.GetComponent<CardMovement>().targetPosition = newPosition;
            }
        }
    }
}
