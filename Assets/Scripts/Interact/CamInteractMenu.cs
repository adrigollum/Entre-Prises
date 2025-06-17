using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CamInteractMenu : MonoBehaviour
{
    private Camera mainCamera;
    public DeckSelectionPositioner deckSelectionPositioner;
    public DeckSelectionPositioner notDeckSelectionPositioner;

    public GameObject cardSelected;

    public float minCardDistance = 1.5f;
    public float maxCardDistance = 3f;
    public float cardDistance = 2f;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
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
                // if (cardArea == CardMovement.CardArea.PlayingArea)
                // {
                //     gameTurn.PlayCard(cardSelected);
                // }
                // else if (cardArea == CardMovement.CardArea.DiscardArea)
                // {
                //     gameTurn.DiscardCard(cardSelected);
                // }
                deckSelectionPositioner.RepositionAllCards();
                notDeckSelectionPositioner.RepositionAllCards();
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
                if (hit.collider == null)
                {
                    return; // No collider hit
                }
                Debug.DrawLine(ray.origin, hit.point, Color.red);

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
                float procCardDistance = Mathf.Abs(cardDistance * (mousePosition.x - Screen.width / 2) / (Screen.width / 2));
                procCardDistance = Mathf.Clamp(procCardDistance, minCardDistance, maxCardDistance);
                Vector3 newPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, procCardDistance));
                cardSelected.GetComponent<CardMovement>().targetPosition = newPosition;
            }
        }
    }
}
