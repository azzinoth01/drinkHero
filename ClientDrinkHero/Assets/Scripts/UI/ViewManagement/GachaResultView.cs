using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaResultView : View
{
    [SerializeField] private GachaData _gachaData;

    [SerializeField] private Button _backButton;
    [SerializeField] private Sprite _backButtonClicked;
    private Sprite _backButtonInitial;


    [SerializeField] private Button multiPullButton;
    [SerializeField] private Button singlePullButton;

    [SerializeField] TextMeshProUGUI _singlePullCost;
    [SerializeField] TextMeshProUGUI _multiPullCost;

    [SerializeField] private List<GachaResultDisplayItem> _displayList;

    public override void Initialize() {

        _backButton.onClick.AddListener(ViewTweener.ButtonClickTween(_backButton,
           _backButtonClicked,() => ViewManager.ShowLast()));

        _backButtonInitial = _backButton.image.sprite;


        multiPullButton.onClick.AddListener(ViewTweener.ButtonClickTween(multiPullButton,
           multiPullButton.image.sprite,() => { MultiPull(); }));

        singlePullButton.onClick.AddListener(ViewTweener.ButtonClickTween(singlePullButton,
            singlePullButton.image.sprite,() => { SinglePull(); }));

        _singlePullCost.text = _gachaData.CostSingelPull.ToString();
        _multiPullCost.text = _gachaData.MultiPullAmount.ToString();

        _singlePullCost.text = _gachaData.CostSingelPull.ToString();
        _multiPullCost.text = _gachaData.CostMultiPull.ToString();
    }


    public void PopulateDisplayList(List<ScriptableObject> pullResults) {

        int i = 0;
        foreach(GachaResultDisplayItem item in _displayList) {
            if(pullResults.Count > i) {
                item.SetData(pullResults[i]);
            }
            else {
                item.SetEmpty();
            }

            i = i + 1;
        }
    }
    private void MultiPull() {

        if(GameDataInstance.Instance.UserSave.Gold < _gachaData.CostMultiPull) {
            return;
        }
        GameDataInstance.Instance.UserSave.Gold = GameDataInstance.Instance.UserSave.Gold - _gachaData.CostMultiPull;
        DisableGachaButtons();
        List<ScriptableObject> result = GachaPullHelper.StartPull(_gachaData.MultiPullAmount,_gachaData);
        if(result == null) {
            GameDataInstance.Instance.UserSave.Gold = GameDataInstance.Instance.UserSave.Gold + _gachaData.CostMultiPull;
            GameDataInstance.Instance.UserSave.Save();
        }

        PopulateDisplayList(result);
        EnableGachaButtons();
    }

    private void SinglePull() {
        if(GameDataInstance.Instance.UserSave.Gold < _gachaData.CostSingelPull) {
            return;
        }
        GameDataInstance.Instance.UserSave.Gold = GameDataInstance.Instance.UserSave.Gold - _gachaData.CostSingelPull;
        DisableGachaButtons();
        List<ScriptableObject> result = GachaPullHelper.StartPull(1,_gachaData);
        if(result == null) {
            GameDataInstance.Instance.UserSave.Gold = GameDataInstance.Instance.UserSave.Gold + _gachaData.CostSingelPull;
            GameDataInstance.Instance.UserSave.Save();
        }

        PopulateDisplayList(result);
        EnableGachaButtons();
    }

    public override void Show() {
        base.Show();
        _backButton.image.sprite = _backButtonInitial;
    }

    private void DisableGachaButtons() {
        singlePullButton.interactable = false;
        multiPullButton.interactable = false;
    }

    private void EnableGachaButtons() {
        singlePullButton.interactable = true;
        multiPullButton.interactable = true;
    }
}
