using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaMenuView : View
{
    [Header("Ui Buttons")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button multiPullButton;
    [SerializeField] private Button singlePullButton;
    [SerializeField] private Button optionsMenuButton;
    
    private int _requestId;
    private int _multiPullRequestId;
    private int _singlePullRequestId;
    
    private bool _dataIsLoading;
    private ResponsMessageObject _response;
    private PullHistoryDatabase _pullHistory;
    
    public override void Initialize()
    {
        backButton.onClick.AddListener(() => SceneLoader.Load(GameSceneEnum.MainMenuScene));
        optionsMenuButton.onClick.AddListener(() => ViewManager.Show<OptionsMenuView>());
        
        multiPullButton.onClick.AddListener(() => MultiPull());
        singlePullButton.onClick.AddListener(() => SinglePull());
    }

    private void MultiPull()
    {
        DisableGachaButtons();
        string request = ClientFunctions.GachaMultiPull();
        _requestId = HandleRequests.Instance.HandleRequest(request, typeof(ResponsMessageObject));
    }

    private void SinglePull()
    {
        DisableGachaButtons();
        string request = ClientFunctions.PullGachaSingel();
        _requestId = HandleRequests.Instance.HandleRequest(request, typeof(ResponsMessageObject));
    } 
    
    private void Update()
    {
        if (_dataIsLoading == false) 
        {
            return;
        }
        
        if (HandleRequests.Instance.RequestDataStatus[_requestId] == DataRequestStatusEnum.Recieved) 
        {
            List<ResponsMessageObject> list = ResponsMessageObject.CreateObjectDataFromString(HandleRequests.Instance.RequestData[_requestId]);
            _response = list[0];
            
            HandleRequests.Instance.RequestDataStatus[_requestId] = DataRequestStatusEnum.RecievedAccepted;
        }
        
        if (_response.Message == "SUCCESS")
        {
            // 1-Singlepull 10-Multipull
            string request = ClientFunctions.GetLastPullResult(10);
            _multiPullRequestId = HandleRequests.Instance.HandleRequest(request, typeof(PullHistoryDatabase));
        }
        
        if (HandleRequests.Instance.RequestDataStatus[_multiPullRequestId] == DataRequestStatusEnum.Recieved) 
        {
            List<PullHistoryDatabase> list = PullHistoryDatabase.CreateObjectDataFromString(HandleRequests.Instance.RequestData[_multiPullRequestId]);
            
            foreach (var pull in list)
            {
                if (pull.Type == "Hero")
                {
                    Debug.Log("You Unlocked a Hero!");
                    //use pull.TypeId to check HeroList and print Hero name
                }
                else if (pull.Type == "Item")
                {
                    Debug.Log("You an Item!");
                    //use pull.TypeId to check ItemList and print Item name
                }
            }
            //_pullHistory.Type "Hero", "Item"
            //_pullHistory.TypeID is the id in the table, used to discern which exact item or hero was pulled
            //get all heroes/items in awake, then use id to tell which is which after pull(s)
            
            HandleRequests.Instance.RequestDataStatus[_multiPullRequestId] = DataRequestStatusEnum.RecievedAccepted;
        }
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
