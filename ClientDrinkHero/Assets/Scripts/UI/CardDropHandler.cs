using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDropHandler : MonoBehaviour, IDropHandler {
    private BattleView _battleView;
    public static event Action OnHideDropZone;

    private void Awake() {
        _battleView = ViewManager.Instance.GetView<BattleView>();
    }

    public void OnDrop(PointerEventData eventData) {

        Debug.Log("CARD DROPED");

        OnHideDropZone?.Invoke();

        _battleView = ViewManager.Instance.GetView<BattleView>();
        CardView cardView = _battleView.playerCardDummy;




        if (cardView != null) {

            int cardIndex = cardView.HandIndex;

            if (cardIndex == -1) {
                return;
            }

            _battleView = ViewManager.Instance.GetView<BattleView>();

            if (_battleView.PlayHandCardOnDrop(cardIndex)) {
                Debug.Log("Card was played successfully.");
                //cardView.ResetCardViewParent();
            }
            else {
                Debug.Log("Not enough Mana.");
                //cardView.ReturnCardToHand();
            }
        }
    }


}