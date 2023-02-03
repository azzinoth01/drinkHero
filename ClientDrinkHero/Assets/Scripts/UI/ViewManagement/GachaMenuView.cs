using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaMenuView : View {
    [Header("Ui Buttons")]
    [SerializeField]
    private Button backButton;

    [SerializeField] private Button multiPullButton;
    [SerializeField] private Button singlePullButton;
    [SerializeField] private Button optionsMenuButton;

    [SerializeField] private Sprite backButtonClicked;

    private int _requestId;
    private int _multiPullRequestId;
    //private int _singlePullRequestId;

    private int _gachaInfoRequestId;

    private bool _dataIsLoading;
    private ResponsMessageObject _response;
    private PullHistoryDatabase _pullHistory;

    [SerializeField] GachaResultView _gachaResultView;

    [SerializeField] TextMeshProUGUI _singlePullCost;
    [SerializeField] TextMeshProUGUI _multiPullCost;


    private GachaDatabase _gachaInfo;

    public override void Initialize() {

        string request = ClientFunctions.GetGachaInfo();
        _gachaInfoRequestId = HandleRequests.Instance.HandleRequest(request, typeof(GachaDatabase));
        NetworkDataContainer.Instance.WaitForServer.AddWaitOnServer();




        optionsMenuButton.onClick.AddListener(ViewTweener.ButtonClickTween(optionsMenuButton,
            optionsMenuButton.image.sprite, () => ViewManager.Show<OptionsMenuView>()));

        backButton.onClick.AddListener(ViewTweener.ButtonClickTween(backButton,
            backButtonClicked, () => SceneLoader.Load(GameSceneEnum.MainMenuScene)));

        multiPullButton.onClick.AddListener(ViewTweener.ButtonClickTween(multiPullButton,
            multiPullButton.image.sprite, () => MultiPull()));

        singlePullButton.onClick.AddListener(ViewTweener.ButtonClickTween(singlePullButton,
            singlePullButton.image.sprite, () => SinglePull()));

        AudioController.Instance.PlayAudio(AudioType.MainMenuTheme, true, 0f);
    }

    private void MultiPull() {
        DisableGachaButtons();
        var request = ClientFunctions.GachaMultiPull();
        _requestId = HandleRequests.Instance.HandleRequest(request, typeof(ResponsMessageObject));
        NetworkDataContainer.Instance.WaitForServer.AddWaitOnServer();
        StartCoroutine(WaitForPullStatus(10));
    }

    private void SinglePull() {
        DisableGachaButtons();
        var request = ClientFunctions.PullGachaSingel();
        _requestId = HandleRequests.Instance.HandleRequest(request, typeof(ResponsMessageObject));
        NetworkDataContainer.Instance.WaitForServer.AddWaitOnServer();
        StartCoroutine(WaitForPullStatus(1));
    }

    private void OnEnable() {
        StartCoroutine(WaitForGachaInfo());
    }

    private IEnumerator WaitForGachaInfo() {
        while (HandleRequests.Instance.RequestDataStatus[_gachaInfoRequestId] != DataRequestStatusEnum.RecievedAccepted) {
            if (HandleRequests.Instance.RequestDataStatus[_gachaInfoRequestId] == DataRequestStatusEnum.Recieved) {
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



    private IEnumerator WaitForPullStatus(int pullAmount) {

        while (HandleRequests.Instance.RequestDataStatus[_requestId] != DataRequestStatusEnum.RecievedAccepted) {
            if (HandleRequests.Instance.RequestDataStatus[_requestId] == DataRequestStatusEnum.Recieved) {
                var list = ResponsMessageObject.CreateObjectDataFromString(HandleRequests.Instance.RequestData[_requestId]);
                _response = list[0];

                HandleRequests.Instance.RequestDataStatus[_requestId] = DataRequestStatusEnum.RecievedAccepted;

                NetworkDataContainer.Instance.WaitForServer.FinishedWaitOnServer();
                if (_response.Message == "SUCCESS") {
                    // 1-Singlepull 10-Multipull
                    var request = ClientFunctions.GetLastPullResult(pullAmount);

                    _multiPullRequestId = HandleRequests.Instance.HandleRequest(request, typeof(PullHistoryDatabase));
                    NetworkDataContainer.Instance.WaitForServer.AddWaitOnServer();

                    string requestUserDataUpdate = ClientFunctions.GetUserData();
                    UserSingelton.Instance.UserObject.UpdateUserDataRequest(requestUserDataUpdate);

                    StartCoroutine(WaitForPullHistory());
                    yield break;
                }
                else {
                    Debug.Log("Pull FAILED");
                    EnableGachaButtons();
                    yield break;
                }
            }
            yield return null;

        }
    }

    private IEnumerator WaitForPullHistory() {

        while (HandleRequests.Instance.RequestDataStatus[_multiPullRequestId] != DataRequestStatusEnum.RecievedAccepted) {
            if (HandleRequests.Instance.RequestDataStatus[_multiPullRequestId] == DataRequestStatusEnum.Recieved) {

                HandleRequests.Instance.RequestDataStatus[_multiPullRequestId] = DataRequestStatusEnum.RecievedAccepted;
                List<PullHistoryDatabase> list = PullHistoryDatabase.CreateObjectDataFromString(HandleRequests.Instance.RequestData[_multiPullRequestId]);

                HandleRequests.Instance.RequestDataStatus[_multiPullRequestId] = DataRequestStatusEnum.RecievedAccepted;
                NetworkDataContainer.Instance.WaitForServer.FinishedWaitOnServer();


                ViewManager.Show(_gachaResultView);

                _gachaResultView.PopulateDisplayList(list);
                EnableGachaButtons();
                yield break;
            }
            yield return null;

        }
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