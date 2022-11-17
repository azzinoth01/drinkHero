using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaUIManager : MonoBehaviour
{
    [SerializeField] private RectTransform _disclaimerPanel, _helpPanel;

    public void LoadMainMenuScene()
    {
        SceneLoader.Load(GameScene.MainMenu);
    }

    public void ShowHelpPanel()
    {
        _helpPanel.gameObject.SetActive(true);
    }

    public void HideHelpPanel()
    {
        _helpPanel.gameObject.SetActive(false);
    }

    public void ShowDisclaimerPanel()
    {
        _disclaimerPanel.gameObject.SetActive(true);
    }

    public void HideDisclaimerPanel()
    {
        _disclaimerPanel.gameObject.SetActive(false);
    }
}