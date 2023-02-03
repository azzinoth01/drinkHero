using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaResultView : View {
    [SerializeField] private Button _backButton;
    [SerializeField] private Sprite _backButtonClicked;
    private Sprite _backButtonInitial;

    [SerializeField] private TextMeshProUGUI _loadingText;

    [SerializeField] private GameObject _loadingPanel;



    [SerializeField] private List<GachaResultDisplayItem> _displayList;



    public override void Initialize() {
        ViewTweener.PulseTextTween(_loadingText);

        _backButton.onClick.AddListener(ViewTweener.ButtonClickTween(_backButton,
           _backButtonClicked, () => ViewManager.ShowLast()));

        _backButtonInitial = _backButton.image.sprite;

        ShowLoadingPanel();
    }


    public void PopulateDisplayList(List<PullHistoryDatabase> pulls) {

        int i = 0;
        foreach (GachaResultDisplayItem item in _displayList) {
            if (pulls.Count >= i) {
                item.SetData(pulls[i]);
            }
            else {
                item.SetEmpty();
            }

            i = i + 1;
        }

        ShowLoadingPanel();
    }





    private void Update() {
        bool check = true;
        foreach (GachaResultDisplayItem item in _displayList) {
            check = check & item.Loaded;
        }
        if (check == true) {
            HideLoadingPanel();
        }
        else {
            ShowLoadingPanel();
        }
    }

    private void ShowLoadingPanel() {
        _loadingPanel.SetActive(true);
    }

    private void HideLoadingPanel() {
        _loadingPanel.SetActive(false);
    }

    public override void Show() {
        base.Show();
        _backButton.image.sprite = _backButtonInitial;
    }
}
