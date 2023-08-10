using System.Collections;
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

    [SerializeField] private Button multiPullButton;
    [SerializeField] private Button singlePullButton;

    [SerializeField] TextMeshProUGUI _singlePullCost;
    [SerializeField] TextMeshProUGUI _multiPullCost;

    [SerializeField] private List<GachaResultDisplayItem> _displayList;

    private int _requestId;
    private int _multiPullRequestId;
    private ResponsMessageObject _response;


    public override void Initialize() {
        ViewTweener.PulseTextTween(_loadingText);

        _backButton.onClick.AddListener(ViewTweener.ButtonClickTween(_backButton,
           _backButtonClicked, () => ViewManager.ShowLast()));

        _backButtonInitial = _backButton.image.sprite;

        ShowLoadingPanel();


        multiPullButton.onClick.AddListener(ViewTweener.ButtonClickTween(multiPullButton,
           multiPullButton.image.sprite, () => { MultiPull(); }));

        singlePullButton.onClick.AddListener(ViewTweener.ButtonClickTween(singlePullButton,
            singlePullButton.image.sprite, () => { SinglePull(); }));
    }


    public void PopulateDisplayList(List<PullHistoryDatabase> pulls) {

        int i = 0;
        foreach (GachaResultDisplayItem item in _displayList) {
            if (pulls.Count > i) {
                item.SetData(pulls[i]);
            }
            else {
                item.SetEmpty();
            }

            i = i + 1;
        }

        ShowLoadingPanel();
    }


    private void OnEnable()
    {
        StartCoroutine(WaitForGachaInfo());
    }

    private IEnumerator WaitForGachaInfo()
    {
        string request = ClientFunctions.GetGachaInfo();
        int _gachaInfoRequestId = HandleRequests.Instance.HandleRequest(request, typeof(GachaDatabase));
        GachaDatabase _gachaInfo;
        while (HandleRequests.Instance.RequestDataStatus[_gachaInfoRequestId] != DataRequestStatusEnum.RecievedAccepted)
        {
            if (HandleRequests.Instance.RequestDataStatus[_gachaInfoRequestId] == DataRequestStatusEnum.Recieved)
            {
                 _gachaInfo = GachaDatabase.CreateObjectDataFromString(HandleRequests.Instance.RequestData[_gachaInfoRequestId])[0];


                HandleRequests.Instance.RequestDataStatus[_gachaInfoRequestId] = DataRequestStatusEnum.RecievedAccepted;

                NetworkDataContainer.Instance.WaitForServer.FinishedWaitOnServer();


                _singlePullCost.SetText("x" + _gachaInfo.CostSingelPull);
                _multiPullCost.SetText("x" + _gachaInfo.CostMultiPull);
                yield break;

            }
            yield return null;

        }
    }
    private void MultiPull()
    {
        DisableGachaButtons();
        var request = ClientFunctions.GachaMultiPull();
        _requestId = HandleRequests.Instance.HandleRequest(request, typeof(ResponsMessageObject));
        NetworkDataContainer.Instance.WaitForServer.AddWaitOnServer();
        StartCoroutine(WaitForPullStatus(10));
    }

    private void SinglePull()
    {
        DisableGachaButtons();
        var request = ClientFunctions.PullGachaSingel();
        _requestId = HandleRequests.Instance.HandleRequest(request, typeof(ResponsMessageObject));
        NetworkDataContainer.Instance.WaitForServer.AddWaitOnServer();
        StartCoroutine(WaitForPullStatus(1));
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

    private IEnumerator WaitForPullStatus(int pullAmount)
    {

        while (HandleRequests.Instance.RequestDataStatus[_requestId] != DataRequestStatusEnum.RecievedAccepted)
        {
            if (HandleRequests.Instance.RequestDataStatus[_requestId] == DataRequestStatusEnum.Recieved)
            {
                var list = ResponsMessageObject.CreateObjectDataFromString(HandleRequests.Instance.RequestData[_requestId]);
                _response = list[0];

                HandleRequests.Instance.RequestDataStatus[_requestId] = DataRequestStatusEnum.RecievedAccepted;

                NetworkDataContainer.Instance.WaitForServer.FinishedWaitOnServer();
                if (_response.Message == "SUCCESS")
                {
                    // 1-Singlepull 10-Multipull
                    var request = ClientFunctions.GetLastPullResult(pullAmount);

                    _multiPullRequestId = HandleRequests.Instance.HandleRequest(request, typeof(PullHistoryDatabase));
                    NetworkDataContainer.Instance.WaitForServer.AddWaitOnServer();

                    string requestUserDataUpdate = ClientFunctions.GetUserData();
                    UserSingelton.Instance.UserObject.UpdateUserDataRequest(requestUserDataUpdate);

                    StartCoroutine(WaitForPullHistory());
                    yield break;
                }
                else
                {
                    Debug.Log("Pull FAILED");
                    EnableGachaButtons();
                    yield break;
                }
            }
            yield return null;

        }
    }

    private IEnumerator WaitForPullHistory()
    {

        while (HandleRequests.Instance.RequestDataStatus[_multiPullRequestId] != DataRequestStatusEnum.RecievedAccepted)
        {
            if (HandleRequests.Instance.RequestDataStatus[_multiPullRequestId] == DataRequestStatusEnum.Recieved)
            {

                HandleRequests.Instance.RequestDataStatus[_multiPullRequestId] = DataRequestStatusEnum.RecievedAccepted;
                List<PullHistoryDatabase> list = PullHistoryDatabase.CreateObjectDataFromString(HandleRequests.Instance.RequestData[_multiPullRequestId]);

                HandleRequests.Instance.RequestDataStatus[_multiPullRequestId] = DataRequestStatusEnum.RecievedAccepted;
                NetworkDataContainer.Instance.WaitForServer.FinishedWaitOnServer();

                PopulateDisplayList(list);
                EnableGachaButtons();
                yield break;
            }
            yield return null;

        }
        EnableGachaButtons();
    }



    private void DisableGachaButtons()
    {
        singlePullButton.interactable = false;
        multiPullButton.interactable = false;
    }

    private void EnableGachaButtons()
    {
        singlePullButton.interactable = true;
        multiPullButton.interactable = true;
    }
}
