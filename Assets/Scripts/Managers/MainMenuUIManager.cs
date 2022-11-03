using UnityEngine;

public class MainMenuUIManager : MonoBehaviour {
    [SerializeField] private GameObject _playerOptionsPanel;


    public void LoadBattleScene() {
        SceneLoader.Load(SceneLoader.GameScene.BattleScene);

    }

    public void LoadGachaMenuScene() {
        SceneLoader.Load(SceneLoader.GameScene.GachaMenu);
    }

    public void ShowOptionsPanel() {
        _playerOptionsPanel.SetActive(true);
    }

    public void HideOptionsPanel() {
        _playerOptionsPanel.SetActive(false);
    }
}
