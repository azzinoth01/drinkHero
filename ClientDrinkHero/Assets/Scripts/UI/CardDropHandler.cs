using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDropHandler : MonoBehaviour, IDropHandler
{
    [SerializeField] private float dropDelay = 1f;
    private BattleView _battleView;
    

    private void Awake()
    {
        //_battleView = transform.parent.GetComponent<BattleView>();
        _battleView = ViewManager.Instance.GetView<BattleView>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        //StartCoroutine(ReturnCardToHandDelay(eventData));

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

    private IEnumerator ReturnCardToHandDelay(PointerEventData eventData)
    {
        yield return new WaitForSeconds(dropDelay);
        
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