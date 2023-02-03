using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenuView : View
{
    [Header("Ui Buttons")] [SerializeField]
    private Button battlePreparationMenuButton;

    [SerializeField] private Button gachaMenuButton;
    [SerializeField] private Button drinksMenuButton;
    [SerializeField] private Button optionsMenuButton;
    [SerializeField] private Button creditsMenuButton;

    [SerializeField] private Sprite battlePreparationClicked;
    [SerializeField] private Sprite drinksMenuClicked;
    [SerializeField] private Sprite gachaMenuClicked;
    [SerializeField] private Sprite creditsMenuClicked;
    private Sprite _creditsMenuSprite;
    
    public override void Initialize()
    {
        battlePreparationMenuButton.onClick.AddListener(ViewTweener.ButtonClickTween(battlePreparationMenuButton, 
            battlePreparationClicked, () => SceneLoader.Load(GameSceneEnum.BattlePreparationMenu)));
        
        gachaMenuButton.onClick.AddListener(ViewTweener.ButtonClickTween(gachaMenuButton, 
            gachaMenuClicked, () => SceneLoader.Load(GameSceneEnum.GachaMenuScene)));

        optionsMenuButton.onClick.AddListener(ViewTweener.ButtonClickTween(optionsMenuButton, 
            optionsMenuButton.image.sprite, () => ViewManager.Show<OptionsMenuView>()));
        
        creditsMenuButton.onClick.AddListener(ViewTweener.ButtonClickTween(creditsMenuButton, 
            creditsMenuClicked, () => ViewManager.Show<CreditsView>()));

        AudioController.Instance.PlayAudio(AudioType.MainMenuTheme, true, 0f);

        _creditsMenuSprite = creditsMenuButton.image.sprite;
    }

    public override void Show()
    {
        base.Show();
        creditsMenuButton.image.sprite = _creditsMenuSprite;
    }
}