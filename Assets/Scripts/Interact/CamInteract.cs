using UnityEngine;
using UnityEngine.InputSystem;

public class CamInteract : MonoBehaviour
{
    private Camera mainCamera;
    public GameObject cardSelected;

    public PlayerInfo playerInfo;
    public Inventory inventory;

    public float minCardDistance = 1.5f;
    public float maxCardDistance = 3f;
    public float cardDistance = 2f;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        playerInfo = GetComponent<PlayerInfo>();
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
                            playerInfo.PlayCard(cardSelected);
                        }
                        inventory.RepositionAllCards();
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
