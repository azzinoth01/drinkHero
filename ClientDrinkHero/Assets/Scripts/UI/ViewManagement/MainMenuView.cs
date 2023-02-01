using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : View
{
    [Header("Ui Buttons")] [SerializeField]
    private Button battlePreparationMenuButton;

    [SerializeField] private Button gachaMenuButton;
    [SerializeField] private Button drinksMenuButton;
    [SerializeField] private Button optionsMenuButton;

    public override void Initialize()
    {
        battlePreparationMenuButton.onClick.AddListener(() => SceneLoader.Load(GameSceneEnum.BattlePreparationMenu));
        gachaMenuButton.onClick.AddListener(() => SceneLoader.Load(GameSceneEnum.GachaMenuScene));
        optionsMenuButton.onClick.AddListener(() => ViewManager.Show<OptionsMenuView>());
        
        optionsMenuButton.onClick.AddListener(() => AudioController.Instance.PlayAudio(AudioType.SFXButtonYes));
        
        AudioController.Instance.PlayAudio(AudioType.MainMenuTheme, true, 0f);
    }
}