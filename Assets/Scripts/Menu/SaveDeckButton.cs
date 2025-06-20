using UnityEngine;
using UnityEngine.UI;

public class SaveDeckButton : MenuMovementButton
{
    public DeckSelectionPositioner deckSelectionPositioner;
    public DeckSelectionPositioner notDeckSelectionPositioner;

    void Start()
    {
        Button button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("Button component not found on this GameObject.");
            return;
        }

        button.onClick.AddListener(() =>
        {
            if (deckSelectionPositioner.allCards.Count != deckSelectionPositioner.maxDeckSize)
            {
                Debug.LogWarning("Deck size does not match the maximum deck size. Please adjust your deck.");
                return;
            }

            StaticDeckSave.SaveDeck(StaticDeckSave.DeckType.Deck, deckSelectionPositioner.GetCount());
            StaticDeckSave.SaveDeck(StaticDeckSave.DeckType.NotDeck, notDeckSelectionPositioner.GetCount());

            cameraController.GoToPoint(waypointIndex);
            Debug.Log("Decks saved successfully.");
        });
    }
}
