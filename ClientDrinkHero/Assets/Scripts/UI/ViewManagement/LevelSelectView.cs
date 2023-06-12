using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectView : View
{
    [Header("Ui Buttons")]

    [SerializeField] private Button backButton;
    [SerializeField] private Sprite backButtonClicked;

    [SerializeField] private Button levelButtonPrefab;
    [SerializeField] private Transform levelButtonsPanel;
    [SerializeField] private int levelCount = 21;

    private Button [] levelButtons;

    public override void Initialize()
    {
        backButton.onClick.AddListener(ViewTweener.ButtonClickTween(backButton,
            backButtonClicked, () => SceneLoader.Load(GameSceneEnum.MainMenuScene)));

        for (int i = 0; i < levelCount; i++)
        {
            Button newLevelButton = Instantiate(levelButtonPrefab, levelButtonsPanel);
            newLevelButton.GetComponentInChildren<TextMeshProUGUI>().text = (i + 1).ToString();

            newLevelButton.interactable = PlayerPrefs.GetInt("MaxLevel", 1) >= (i+1);

            int currentLevel = i + 1;
            newLevelButton.onClick.AddListener(ViewTweener.ButtonClickTween(newLevelButton,
            newLevelButton.image.sprite, () => { 
                SceneLoader.Load(GameSceneEnum.BattleScene);
                PlayerPrefs.SetString("GameMode", "Level");
                PlayerPrefs.SetInt("CurrentLevel", currentLevel);
            }));
        }
    }
}
