using UnityEngine;
using UnityEngine.UI;

public class GachaMenuView : View
{
    [Header("Ui Buttons")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button optionsMenuButton;
    public override void Initialize()
    {
        backButton.onClick.AddListener(() => SceneLoader.Load(GameSceneEnum.MainMenuScene));
        optionsMenuButton.onClick.AddListener(() => ViewManager.Show<OptionsMenuView>());
    }
}
