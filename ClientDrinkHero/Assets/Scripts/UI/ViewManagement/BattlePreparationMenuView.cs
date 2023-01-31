using UnityEngine;
using UnityEngine.UI;

public class BattlePreparationMenuView : View
{
    [Header("Ui Buttons")] [SerializeField]
    private Button enterbattleButton;

    [SerializeField] private Button backButton;
    [SerializeField] private Button optionsMenuButton;

    [SerializeField] private Button[] characterSlots;


    public override void Initialize()
    {
        TeamController.OnTeamReady += ToggleBattleButton;

        enterbattleButton.onClick.AddListener(() => SceneLoader.Load(GameSceneEnum.BattleScene));
        backButton.onClick.AddListener(() => SceneLoader.Load(GameSceneEnum.MainMenuScene));
        optionsMenuButton.onClick.AddListener(() => ViewManager.Show<OptionsMenuView>());

        foreach (var slot in characterSlots) slot.onClick.AddListener(() => ViewManager.Show<CharacterSelectView>());
    }

    private void ToggleBattleButton(bool state)
    {
        enterbattleButton.interactable = state;
    }
}