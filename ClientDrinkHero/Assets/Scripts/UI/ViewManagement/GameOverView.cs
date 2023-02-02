using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverView : View
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private TextMeshProUGUI floorCounter;
    [SerializeField] private TextMeshProUGUI goldCounter;

    public override void Initialize()
    {
        mainMenuButton.onClick.AddListener(() => SceneLoader.Load(GameSceneEnum.MainMenuScene));
    }

    public override void Show()
    {
        base.Show();
        goldCounter.SetText($"x {EnemyObject.GoldGotThisSession}");
        floorCounter.SetText($"x {EnemyObject.LevelData.levelCount}");
    }
}