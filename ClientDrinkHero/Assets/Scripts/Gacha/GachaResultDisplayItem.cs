using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaResultDisplayItem : MonoBehaviour {


    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private Image _characterPortraitImage;

    private LoadSprite _loadSprite;

    private GachaResultView _view;

    private string _showType;
    private PullHistoryDatabase _pullData;

    private int _requestId;
    private bool _loaded;

    public bool Loaded {
        get {
            return _loaded;
        }

    }

    private void Awake() {

        _loadSprite = _characterPortraitImage.GetComponent<LoadSprite>();
        _loaded = false;
    }

    private void Start() {
        if (!_view) {
            _view = ViewManager.Instance.GetView<GachaResultView>();
        }
    }

    public void SetData(PullHistoryDatabase pullData) {
        _pullData = pullData;
        _loaded = false;
        string request;
        if (_pullData == null) {
            SetEmpty();
            return;
        }
        if (_pullData.Type == "Hero") {
            request = ClientFunctions.GetHeroDatabaseByKeyPair("ID\"" + _pullData.TypeID + "\"");
            _requestId = HandleRequests.Instance.HandleRequest(request, typeof(HeroDatabase));
            NetworkDataContainer.Instance.WaitForServer.AddWaitOnServer();
            StartCoroutine(WaitForData());
            Debug.Log("You Unlocked a Hero!");
        }
        //use pull.TypeId to check HeroList and print Hero name
        else if (_pullData.Type == "Item") {
            request = ClientFunctions.GetUpgradeItemDatabaseByKey("ID\"" + _pullData.TypeID + "\"");
            _requestId = HandleRequests.Instance.HandleRequest(request, typeof(UpgradeItemDatabase));
            NetworkDataContainer.Instance.WaitForServer.AddWaitOnServer();

            StartCoroutine(WaitForData());
            Debug.Log("You got an Item!");
        }
        else {
            SetEmpty();
        }

    }

    private IEnumerator WaitForData() {
        while (HandleRequests.Instance.RequestDataStatus[_requestId] != DataRequestStatusEnum.RecievedAccepted) {
            if (HandleRequests.Instance.RequestDataStatus[_requestId] == DataRequestStatusEnum.Recieved) {

                if (_pullData.Type == "Hero") {

                    HeroDatabase hero = HeroDatabase.CreateObjectDataFromString(HandleRequests.Instance.RequestData[_requestId])[0];
                    _loadSprite.SpritePathSufix = "_Slot.png";
                    _loadSprite.LoadNewSprite(hero.SpritePath);
                    _itemName.SetText(hero.Name);
                }

                else if (_pullData.Type == "Item") {

                    UpgradeItemDatabase item = UpgradeItemDatabase.CreateObjectDataFromString(HandleRequests.Instance.RequestData[_requestId])[0];
                    _loadSprite.SpritePathSufix = ".png";
                    _loadSprite.LoadNewSprite(item.SpritePath);
                    _itemName.SetText(item.Name);
                }


                HandleRequests.Instance.RequestDataStatus[_requestId] = DataRequestStatusEnum.RecievedAccepted;
                NetworkDataContainer.Instance.WaitForServer.FinishedWaitOnServer();
                _loaded = true;
                yield break;
            }
            yield return null;

        }
    }

    public void SetEmpty() {
        //just in case
        _pullData = new PullHistoryDatabase();
        _pullData = null;

        _loadSprite.UnloadSprite();

        _loaded = true;

    }


}
