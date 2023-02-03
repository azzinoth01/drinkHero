using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDropHandler : MonoBehaviour, IDropHandler {
    [SerializeField] private float dropDelay = 1f;
    private BattleView _battleView;


    private void Awake() {
        //_battleView = transform.parent.GetComponent<BattleView>();
        _battleView = ViewManager.Instance.GetView<BattleView>();
    }

    public void OnDrop(PointerEventData eventData) {
        //StartCoroutine(ReturnCardToHandDelay(eventData));

        var droppedCard = eventData.pointerDrag;
        var cardView = droppedCard.GetComponent<CardView>();

        if (droppedCard != null) {
            Debug.Log("Dropped object was: " + eventData.pointerDrag);
            var cardIndex = cardView.HandIndex;
            Debug.Log("Card Index " + cardIndex);

            Debug.Log("playerhand: " + cardView.HandCards);

            Debug.Log("battleview " + _battleView);


            //IHandCards playerHand = UIDataContainer.Instance.Player.GetHandCards();

            //Debug.Log("playerhand " + playerHand);

            //bool cardWasPlayed = playerHand.PlayHandCard(cardIndex);
            //Debug.Log("card was played");
            //if (cardWasPlayed) {
            //    droppedCard.SetActive(false);
            //    //_battleView.currentPlayerHand[cardIndex].gameObject.SetActive(false);
            //    //Button button = _battleView.currentPlayerHand[cardIndex].GetComponent<Button>();
            //    //Debug.Log("button " + button);
            //    //button.onClick.RemoveAllListeners();
            //    Debug.Log("set active to false");
            //    DisolveCard disolveCard = droppedCard.GetComponent<DisolveCard>();
            //    Debug.Log("disolve " + disolveCard);
            //    disolveCard.enabled = true;
            //    _battleView = ViewManager.Instance.GetView<BattleView>();
            //    _battleView.UpdateHandCards();
            //}
            _battleView = ViewManager.Instance.GetView<BattleView>();

            if (_battleView.PlayHandCardOnDrop(cardIndex)) {
                Debug.Log("Card was played successfully.");
                cardView.ResetCardViewParent();
            }
            //if (cardWasPlayed) {
            //    Debug.Log("Card was played successfully.");
            //    cardView.ResetCardViewParent();
            //}
            else {
                Debug.Log("Not enough Mana.");
                cardView.ReturnCardToHand();
            }
        }
    }

    private IEnumerator ReturnCardToHandDelay(PointerEventData eventData) {
        yield return new WaitForSeconds(dropDelay);

        var droppedCard = eventData.pointerDrag;
        var cardView = droppedCard.GetComponent<CardView>();

        if (droppedCard != null) {
            Debug.Log("Dropped object was: " + eventData.pointerDrag);
            var cardIndex = cardView.HandIndex;
            _battleView = ViewManager.Instance.GetView<BattleView>();
            if (_battleView.PlayHandCardOnDrop(cardIndex)) {
                Debug.Log("Card was played successfully.");
                cardView.ResetCardViewParent();
            }
            else {
                Debug.Log("Not enough Mana.");
                cardView.ReturnCardToHand();
            }
        }
    }
}