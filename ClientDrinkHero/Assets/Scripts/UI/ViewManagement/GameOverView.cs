using UnityEngine;
using UnityEngine.UI;

public class GameOverView : View
{
    [SerializeField] private Button mainMenuButton;

    public override void Initialize()
    {
        mainMenuButton.onClick.AddListener(() => SceneLoader.Load(GameSceneEnum.MainMenuScene));
    }
}