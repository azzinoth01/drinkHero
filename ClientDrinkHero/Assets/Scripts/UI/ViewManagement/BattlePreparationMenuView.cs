using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BattlePreparationMenuView : View
{
    [Header("Ui Buttons")] [SerializeField]
    private Button enterbattleButton;

    [SerializeField] private Button backButton;
    [SerializeField] private Button optionsMenuButton;
    [SerializeField] private Sprite backButtonClicked;
    
    [SerializeField] private Button[] characterSlots;


    public override void Initialize()
    {
        TeamController.OnTeamReady += ToggleBattleButton;

        enterbattleButton.onClick.AddListener(ViewTweener.ButtonClickTween(enterbattleButton, 
            enterbattleButton.image.sprite, () => SceneLoader.Load(GameSceneEnum.BattleScene)));
        
        backButton.onClick.AddListener(ViewTweener.ButtonClickTween(backButton, 
            backButtonClicked, () => SceneLoader.Load(GameSceneEnum.MainMenuScene)));

        optionsMenuButton.onClick.AddListener(ViewTweener.ButtonClickTween(optionsMenuButton, 
            optionsMenuButton.image.sprite, () => ViewManager.Show<OptionsMenuView>()));
        
        foreach (var slot in characterSlots)
        {
            Transform transform = slot.gameObject.transform;
            slot.onClick.AddListener(ViewTweener.ScaleTransformTween(transform, () => ViewManager.Show<CharacterSelectView>()));
        }
        
        AudioController.Instance.PlayAudio(AudioType.MainMenuTheme, true);
    }

    private void OnDestroy()
    {
        TeamController.OnTeamReady -= ToggleBattleButton;
    }

    private void ToggleBattleButton(bool state)
    {
        enterbattleButton.interactable = state;
    }
}