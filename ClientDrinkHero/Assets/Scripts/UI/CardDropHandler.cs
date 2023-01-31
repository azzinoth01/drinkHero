using UnityEngine;
using UnityEngine.EventSystems;

public class CardDropHandler : MonoBehaviour, IDropHandler
{
    private BattleView _battleView;

    private void Awake()
    {
        _battleView = transform.parent.GetComponent<BattleView>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        var droppedCard = eventData.pointerDrag;
        var cardView = droppedCard.GetComponent<CardView>();

        if (droppedCard != null)
        {
            Debug.Log("Dropped object was: " + eventData.pointerDrag);
            var cardIndex = cardView.HandIndex;

            if (_battleView.PlayHandCardOnDrop(cardIndex))
            {
                Debug.Log("Card was played successfully.");
                cardView.ResetCardViewParent();
            }
            else
            {
                Debug.Log("Not enough Mana.");
                cardView.ReturnCardToHand();
            }
        }
    }
}