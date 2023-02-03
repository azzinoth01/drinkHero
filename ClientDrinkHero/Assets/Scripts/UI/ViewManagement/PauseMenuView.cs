using UnityEngine;
using UnityEngine.UI;

public class PauseMenuView : View
{
    [SerializeField] private Button returnButton;
    [SerializeField] private Button mainMenuButton;

    public override void Initialize()
    {
        returnButton.onClick.AddListener(ViewTweener.ButtonClickTween(returnButton, 
            returnButton.image.sprite, () => ViewManager.ShowLast()));
        
        mainMenuButton.onClick.AddListener(ViewTweener.ButtonClickTween(mainMenuButton, 
            mainMenuButton.image.sprite, () => SceneLoader.Load(GameSceneEnum.MainMenuScene)));
    }
}