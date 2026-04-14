using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCardTabView : View
{

    [SerializeField] private TextMeshProUGUI _characterNameLabel;
    [SerializeField] private Image _characterFactionImage;
    [SerializeField] private Image _characterPortraitImage;
    [SerializeField] private Button _backButton;
    [SerializeField] private Sprite _backButtonClicked;

    [SerializeField] private List<GameObject> _tabList;
    private int _currentTab;

    private HeroObject _hero;
    private Sprite _backButtonInitial;

    private LoadSprite _loadSprite;
    public static event Action OnZoomReset;


    public HeroObject Hero {
        get {
            return _hero;
        }


    }
    public override void Initialize() {
        _backButton.onClick.AddListener(ViewTweener.ButtonClickTween(_backButton,_backButtonClicked,() => ViewManager.ShowLast()));

        _backButtonInitial = _backButton.image.sprite;

        _loadSprite = _characterPortraitImage.GetComponent<LoadSprite>();
    }

    public override void Show() {
        base.Show();
        ShowTab(0);
        _backButton.image.sprite = _backButtonInitial;
    }

    public override void Hide() {
        base.Hide();
        OnZoomReset?.Invoke();
    }

    public void ShowTab(int tabIndex) {
        if(_tabList.Count > tabIndex) {
            _tabList[_currentTab].SetActive(false);
            _currentTab = tabIndex;
            _tabList[_currentTab].SetActive(true);
        }
    }

    public void LoadCharacterData(string id) {
        List<HeroObject> ownedHeroes = GameDataInstance.Instance.OwnedHeroes;
        foreach(HeroObject hero in ownedHeroes) {
            if(hero.Id == id) {
                _hero = hero;
            }
        }
        if(string.IsNullOrEmpty(_hero.Id) == true) {
            return;
        }
        if(_loadSprite == null) {
            _loadSprite = _characterPortraitImage.GetComponent<LoadSprite>();
        }
        _loadSprite.LoadNewSprite(_hero.SpritePath);
        _characterNameLabel.SetText(_hero.Name);
        ViewManager.Show<CharacterCardTabView>();
    }
}
