using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : View
{
    [Header("Ui Buttons")] [SerializeField]
    private Button battlePreparationMenuButton;

    [SerializeField] private Button gachaMenuButton;
    [SerializeField] private Button drinksMenuButton;
    [SerializeField] private Button optionsMenuButton;

    [SerializeField] private Sprite battlePreparationClicked;
    [SerializeField] private Sprite drinksMenuClicked;
    [SerializeField] private Sprite gachaMenuClicked;
    
    public override void Initialize()
    {
        battlePreparationMenuButton.onClick.AddListener(() =>
        {
            AudioController.Instance.PlayAudio(AudioType.SFXButtonYes);

            battlePreparationMenuButton.image.sprite = battlePreparationClicked;
            
            Sequence sequence = DOTween.Sequence();
            RectTransform rectTransform = battlePreparationMenuButton.GetComponent<RectTransform>();

            sequence.Append(rectTransform.DOScale(0.85f,0.1f))
                .SetEase(Ease.InBounce)
                .Append(rectTransform.DOScale(1,0.1f))
                .SetEase(Ease.OutSine).OnComplete(() => SceneLoader.Load(GameSceneEnum.BattlePreparationMenu));
        });
        
        gachaMenuButton.onClick.AddListener(() => SceneLoader.Load(GameSceneEnum.GachaMenuScene));
        optionsMenuButton.onClick.AddListener(() => ViewManager.Show<OptionsMenuView>());
        
        optionsMenuButton.onClick.AddListener(() => AudioController.Instance.PlayAudio(AudioType.SFXButtonYes));
        
        AudioController.Instance.PlayAudio(AudioType.MainMenuTheme, true, 0f);
    }
}