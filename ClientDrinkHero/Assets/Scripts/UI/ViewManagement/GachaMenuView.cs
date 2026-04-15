using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GachaMenuView : View
{

    [SerializeField] private GachaData _gachaData;

    [Header("Ui Buttons")]
    [SerializeField]
    private Button backButton;

    [SerializeField] private Button multiPullButton;
    [SerializeField] private Button singlePullButton;

    [SerializeField] private Button optionsMenuButton;

    [SerializeField] private Sprite backButtonClicked;
    [SerializeField] GachaResultView _gachaResultView;

    [SerializeField] TextMeshProUGUI _singlePullCost;
    [SerializeField] TextMeshProUGUI _multiPullCost;

    public override void Initialize() {

        optionsMenuButton.onClick.AddListener(ViewTweener.ButtonClickTween(optionsMenuButton,
            optionsMenuButton.image.sprite,() => ViewManager.Show<OptionsMenuView>()));

        backButton.onClick.AddListener(ViewTweener.ButtonClickTween(backButton,
            backButtonClicked,() => SceneManager.LoadScene(GameSceneEnum.MainMenuScene.ToString())));

        multiPullButton.onClick.AddListener(ViewTweener.ButtonClickTween(multiPullButton,
            multiPullButton.image.sprite,() => MultiPull()));

        singlePullButton.onClick.AddListener(ViewTweener.ButtonClickTween(singlePullButton,
            singlePullButton.image.sprite,() => SinglePull()));


        AudioController.Instance.PlayAudio(AudioType.MainMenuTheme,true,0f);

        _singlePullCost.text = _gachaData.CostSingelPull.ToString();
        _multiPullCost.text = _gachaData.CostMultiPull.ToString();
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

        ViewManager.Show(_gachaResultView);

        _gachaResultView.PopulateDisplayList(result);
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

        ViewManager.Show(_gachaResultView);

        _gachaResultView.PopulateDisplayList(result);
        EnableGachaButtons();
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