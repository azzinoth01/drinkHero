using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BattlePreparationMenuView : View
{
    [Header("Ui Buttons")]
    [SerializeField] private Button enterbattleButton;
    [SerializeField] private Button selectLevelButton;
    [SerializeField] private Button changeGameModeButton;

    [SerializeField] private Button backButton;
    [SerializeField] private Button optionsMenuButton;
    [SerializeField] private Sprite backButtonClicked;
    
    [SerializeField] private Button[] characterSlots;

    [SerializeField] private GameObject endlessModePaneln;
    [SerializeField] private GameObject levelModePanel;

    private GameMode currentGameMode;

    private enum GameMode
    {
        Endless,
        Level
    }

    public override void Initialize()
    {
        TeamController.OnTeamReady += ToggleBattleButton;

        enterbattleButton.onClick.AddListener(ViewTweener.ButtonClickTween(enterbattleButton, 
            enterbattleButton.image.sprite, () =>
            {
                SceneLoader.Load(GameSceneEnum.BattleScene);
                PlayerPrefs.SetString("GameMode", "Endless");
            }));
        
        backButton.onClick.AddListener(ViewTweener.ButtonClickTween(backButton, 
            backButtonClicked, () => SceneLoader.Load(GameSceneEnum.MainMenuScene)));

        optionsMenuButton.onClick.AddListener(ViewTweener.ButtonClickTween(optionsMenuButton, 
            optionsMenuButton.image.sprite, () => ViewManager.Show<OptionsMenuView>()));

        selectLevelButton.onClick.AddListener(ViewTweener.ButtonClickTween(selectLevelButton,
            selectLevelButton.image.sprite, () => ViewManager.Show<LevelSelectView>()));

        changeGameModeButton.onClick.AddListener(ViewTweener.ButtonClickTween(changeGameModeButton,
            changeGameModeButton.image.sprite, () => ToggleGameMode()));

        foreach (var slot in characterSlots)
        {
            Transform transform = slot.gameObject.transform;
            slot.onClick.AddListener(ViewTweener.ScaleTransformTween(transform, () => ViewManager.Show<CharacterSelectView>()));
        }
        
        AudioController.Instance.PlayAudio(AudioType.MainMenuTheme, true);
        SetGameMode(GameMode.Endless);
    }

    private void OnDestroy()
    {
        TeamController.OnTeamReady -= ToggleBattleButton;
    }

    private void ToggleBattleButton(bool state)
    {
        enterbattleButton.interactable = state;
        selectLevelButton.interactable = state;
    }

    private void ToggleGameMode()
    {
        SetGameMode(currentGameMode == GameMode.Endless ? GameMode.Level : GameMode.Endless);
    }

    private void SetGameMode(GameMode gameMode)
    {
        currentGameMode = gameMode;
        endlessModePaneln.SetActive(gameMode == GameMode.Endless);
        levelModePanel.SetActive(gameMode == GameMode.Level);
    }
}