using UnityEngine;
using UnityEngine.UI;

public class BattlePreparationMenuView : View
{
    [Header("Ui Buttons")]
    [SerializeField] private Button enterbattleButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button optionsMenuButton;

    public override void Initialize()
    {
        enterbattleButton.onClick.AddListener(() => SceneLoader.Load(GameSceneEnum.BattleScene));
        backButton.onClick.AddListener(() => SceneLoader.Load(GameSceneEnum.MainMenuScene));
        optionsMenuButton.onClick.AddListener(() => ViewManager.Show<OptionsMenuView>());
    }
}
