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
    
    public override void Initialize()
    {
        battlePreparationMenuButton.onClick.AddListener(ButtonClickTween(battlePreparationMenuButton, 
            battlePreparationClicked, () => SceneLoader.Load(GameSceneEnum.BattlePreparationMenu)));
        
        gachaMenuButton.onClick.AddListener(ButtonClickTween(gachaMenuButton, 
            gachaMenuClicked, () => SceneLoader.Load(GameSceneEnum.GachaMenuScene)));
        
        //gachaMenuButton.onClick.AddListener(() => SceneLoader.Load(GameSceneEnum.GachaMenuScene));
        optionsMenuButton.onClick.AddListener(() => ViewManager.Show<OptionsMenuView>());
        
        optionsMenuButton.onClick.AddListener(() => AudioController.Instance.PlayAudio(AudioType.SFXButtonYes));
        
        AudioController.Instance.PlayAudio(AudioType.MainMenuTheme, true, 0f);
    }

    private UnityAction ButtonClickTween(Button button, Sprite clickedSprite, TweenCallback callback, AudioType type=AudioType.SFXButtonYes)
    {
        UnityAction action = () =>
        {
            AudioController.Instance.PlayAudio(type);

            button.image.sprite = clickedSprite;

            Sequence sequence = DOTween.Sequence();
            RectTransform rectTransform = button.GetComponent<RectTransform>();

            sequence.Append(rectTransform.DOScale(0.85f, 0.1f))
                .SetEase(Ease.InBounce)
                .Append(rectTransform.DOScale(1, 0.1f))
                .SetEase(Ease.OutSine).OnComplete(callback);
        };

        return action;
    }
}