using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDropHandler : MonoBehaviour, IDropHandler
{
    private BattleView _battleView;
    public static event Action OnHideDropZone;

    private void Awake()
    {
        _battleView = ViewManager.Instance.GetView<BattleView>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnHideDropZone?.Invoke();
        
        var droppedCard = eventData.pointerDrag;
        var dropPosition = eventData.pointerDrag.transform.position;

        var cardView = droppedCard.GetComponent<CardView>();
        
        _battleView.playerCardDummy.SetPosition(dropPosition);
        
        if (droppedCard != null)
        {
            Debug.Log("Dropped object was: " + eventData.pointerDrag);
            var cardIndex = cardView.HandIndex;
            Debug.Log("Card Index " + cardIndex);

            Debug.Log("playerhand: " + cardView.HandCards);

            Debug.Log("battleview " + _battleView);

            _battleView = ViewManager.Instance.GetView<BattleView>();

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