using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerOptionsPanel;

    public void LoadBuilderScene()
    {
        SceneLoader.Load(GameSceneEnum.DeckBuilder);
    }

    public void LoadGachaMenuScene()
    {
        SceneLoader.Load(GameSceneEnum.GachaMenu);
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