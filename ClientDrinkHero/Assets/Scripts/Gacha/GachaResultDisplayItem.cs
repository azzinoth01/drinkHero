using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaResultDisplayItem : MonoBehaviour
{


    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private Image _characterPortraitImage;

    [SerializeField] private GachaItemAppear _itemAppearEffect;

    private LoadSprite _loadSprite;

    private GachaResultView _view;

    private ScriptableObject _pullData;

    private void Awake() {
        _loadSprite = _characterPortraitImage.GetComponent<LoadSprite>();
    }

    private void Start() {
        if(!_view) {
            _view = ViewManager.Instance.GetView<GachaResultView>();
        }
    }

    public void SetData(ScriptableObject pullData) {
        _pullData = pullData;
        if(_pullData == null) {
            SetEmpty();
            return;
        }
        if(_pullData is HeroData) {

            _itemAppearEffect.ResetState();
            HeroData hero = (HeroData) _pullData;
            _loadSprite.SpritePathSufix = "_Slot.png";
            _loadSprite.LoadNewSprite(hero.SpritePath);
            _itemName.SetText(hero.Name);
            _itemAppearEffect.Play();
        }
        else if(_pullData is UpgradeItemData) {
            _itemAppearEffect.ResetState();
            UpgradeItemData item = (UpgradeItemData) _pullData;
            _loadSprite.SpritePathSufix = ".png";
            _loadSprite.LoadNewSprite(item.SpritePath);
            _itemName.SetText(item.Name);
            _itemAppearEffect.Play();
        }
        else {
            SetEmpty();
        }

    }

    public void SetEmpty() {
        _pullData = null;
        _itemName.SetText("");
        _loadSprite.UnloadSprite();

        _itemAppearEffect.ResetState();
    }
}