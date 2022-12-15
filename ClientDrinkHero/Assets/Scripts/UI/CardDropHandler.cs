using System;
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
        GameObject droppedCard = eventData.pointerDrag;
        
        if (droppedCard != null)
        {
            Debug.Log ("Dropped object was: "  + eventData.pointerDrag);
            int cardIndex = droppedCard.GetComponent<CardView>().HandIndex;

            if (_battleView.PlayHandCardOnDrop(cardIndex))
            {
                Debug.Log("Card was played successfully.");
            }
            else Debug.Log("Not enough Mana.");
        }
    }
}