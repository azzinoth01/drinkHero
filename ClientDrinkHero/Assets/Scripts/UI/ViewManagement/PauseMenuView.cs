using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuView : View
{
    [SerializeField] private Button returnButton;
    [SerializeField] private Button mainMenuButton;

    public override void Initialize()
    {
        returnButton.onClick.AddListener(() => ViewManager.ShowLast());
        mainMenuButton.onClick.AddListener(() => SceneLoader.Load(GameSceneEnum.MainMenuScene));
    }
}
