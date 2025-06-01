using UnityEngine;
using UnityEngine.InputSystem;

public class CamInteract : MonoBehaviour
{
    private Camera mainCamera;
    private GameTurn gameTurn;
    public GameObject cardSelected;

    public float minCardDistance = 1.5f;
    public float maxCardDistance = 3f;
    public float cardDistance = 2f;

    void Start()
    {
        mainCamera = GetComponent<Camera>();

        GameObject gameManager = GameObject.Find("GameManager");
        gameTurn = gameManager.GetComponent<GameTurn>();
    }

    private void HandleCardClick(CardClick hit)
    {
        GameObject card = hit.gameObject;
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (cardSelected == null)
            {
                cardSelected = card;
            }
            else if (cardSelected == card)
            {
                CardMovement.CardArea cardArea = cardSelected.GetComponent<CardMovement>().IsInPlayingArea();
                if (cardArea == CardMovement.CardArea.PlayingArea)
                {
                    gameTurn.PlayCard(cardSelected);
                }
                else if (cardArea == CardMovement.CardArea.DiscardArea)
                {
                    gameTurn.DiscardCard(cardSelected);
                }
                gameTurn.RepositionAllCards();
                cardSelected = null;
            }
        }
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
                if (cardClick != null)
                {
                    HandleCardClick(cardClick);
                }
                else if (cardSelected == null)
                {
                    IClickable clickable = hit.collider.GetComponent<IClickable>();
                    if (clickable != null)
                    {
                        if (Mouse.current.leftButton.wasPressedThisFrame)
                        {
                            clickable.onClick(gameObject, hit.point, mousePosition, IClickable.ClickType.LeftClick, Mouse.current.leftButton.isPressed);
                        }
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
