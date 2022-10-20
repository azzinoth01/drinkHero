using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerOptionsPanel;
    
    
    public void LoadBattleScene()
    {
        SceneLoader.Load(SceneLoader.GameScene.BattleScene);
    }
    
    public void LoadMainMenuScene()
    {
        SceneLoader.Load(SceneLoader.GameScene.MainMenu);
    }
    
    public void ShowOptionsPanel()
    {
        _playerOptionsPanel.SetActive(true);
    }
    
    public void HideOptionsPanel()
    {
        _playerOptionsPanel.SetActive(false);
    }
}
