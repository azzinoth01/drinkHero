using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CharacterCardTabView : View {

    [SerializeField] private TextMeshProUGUI _characterNameLabel;
    [SerializeField] private Image _characterFactionImage;
    [SerializeField] private Image _characterPortraitImage;
    [SerializeField] private Button _backButton;
    [SerializeField] private Sprite _backButtonClicked;

    [SerializeField] private List<GameObject> _tabList;
    private int _currentTab;

    private HeroDatabase _character;


    private Sprite _backButtonInitial;

    private LoadSprite _loadSprite;
    public static event Action OnZoomReset;



    public HeroDatabase Character {
        get {
            return _character;
        }


    }





    public override void Initialize() {
        _backButton.onClick.AddListener(ViewTweener.ButtonClickTween(_backButton,
            _backButtonClicked, () => ViewManager.ShowLast()));

        _backButtonInitial = _backButton.image.sprite;

        _loadSprite = _characterPortraitImage.GetComponent<LoadSprite>();
    }

    public override void Show() {
        base.Show();
        _currentTab = 0;
        _tabList[_currentTab].SetActive(true);
        _backButton.image.sprite = _backButtonInitial;
    }

    public override void Hide() {
        base.Hide();
        OnZoomReset?.Invoke();
    }

    public void ShowTab(int tabIndex) {
        if (_tabList.Count > tabIndex) {
            _tabList[_currentTab].SetActive(false);
            _currentTab = tabIndex;
            _tabList[_currentTab].SetActive(true);
        }
    }

    public void LoadCharacterData(int id) {
        _character = new HeroDatabase();
        foreach (HeroToUserDatabase userHero in CharacterSelectView.UnlockedHeroes) {
            if (userHero.RefHero == id) {
                _character = userHero.Hero;
            }
        }
        if (_character.Id == 0) {
            return;
        }
        if (_loadSprite == null) {
            _loadSprite = _characterPortraitImage.GetComponent<LoadSprite>();
        }
        _loadSprite.LoadNewSprite(_character.SpritePath);
        _characterNameLabel.SetText(_character.Name);
        ViewManager.Show<CharacterCardTabView>();
    }
}
