using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] private GameObject playerOptionsPanel, waitingForConnectionPanel;
    
    private void OnEnable()
    {
        //UIDataContainer.Instance.WaitingPanel.DisplayWaitingPanel += ToggleWaitingPanel;
    }

    private void OnDisable()
    {
        //UIDataContainer.Instance.WaitingPanel.DisplayWaitingPanel -= ToggleWaitingPanel;
    }
    
    private void ToggleWaitingPanel(bool state)
    {
        waitingForConnectionPanel.SetActive(state);
    }
    
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
        playerOptionsPanel.SetActive(true);
    }

    public void HideOptionsPanel()
    {
        playerOptionsPanel.SetActive(false);
    }
}