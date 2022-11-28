using System;
using UnityEngine;

public class DeckBuilderUIManager : MonoBehaviour
{
    public TMPro.TMP_Dropdown[] characterDropdowns;

    [SerializeField] private GameObject waitingForConnectionPanel;
    
    private void OnEnable()
    {
        UIDataContainer.Instance.WaitingPanel.DisplayWaitingPanel += ToggleWaitingPanel;
    }

    private void OnDisable()
    {
        UIDataContainer.Instance.WaitingPanel.DisplayWaitingPanel -= ToggleWaitingPanel;
    }
    
    private void ToggleWaitingPanel(bool state)
    {
        waitingForConnectionPanel.SetActive(state);
    }
    
    public void DisplayCharacterChoice()
    {
        foreach (var dropdown in characterDropdowns)
        {
            Debug.Log(dropdown.itemText);
            
        }
    }

    public void LoadBattleScene()
    {
        SceneLoader.Load(GameSceneEnum.BattleScene);
    }
    
    public void ReturnToMainScene()
    {
        SceneLoader.Load(GameSceneEnum.MainMenu);
    }
}
