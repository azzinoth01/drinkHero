using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : View
{
    [Header("Ui Buttons")]
    [SerializeField]
    private Button battlePreparationMenuButton;

    [SerializeField] private Button gachaMenuButton;
    [SerializeField] private Button drinksMenuButton;
    [SerializeField] private Button optionsMenuButton;
    [SerializeField] private Button creditsMenuButton;

    [SerializeField] private GameObject userNamePanel;
    [SerializeField] private TextMeshProUGUI userNameText;
    [SerializeField] private TMP_InputField userNameInput;
    [SerializeField] private Button enterUserNameButton;
    [SerializeField] private Button submitUserNameButton;

    [SerializeField] private Sprite battlePreparationClicked;
    [SerializeField] private Sprite drinksMenuClicked;
    [SerializeField] private Sprite gachaMenuClicked;
    [SerializeField] private Sprite creditsMenuClicked;

    private Sprite _creditsMenuSprite;

    public override void Initialize() {
        battlePreparationMenuButton.onClick.AddListener(ViewTweener.ButtonClickTween(battlePreparationMenuButton,
            battlePreparationClicked,() => SceneLoader.Load(GameSceneEnum.BattlePreparationMenu)));

        gachaMenuButton.onClick.AddListener(ViewTweener.ButtonClickTween(gachaMenuButton,
            gachaMenuClicked,() => SceneLoader.Load(GameSceneEnum.GachaMenuScene)));

        optionsMenuButton.onClick.AddListener(ViewTweener.ButtonClickTween(optionsMenuButton,
            optionsMenuButton.image.sprite,() => ViewManager.Show<OptionsMenuView>()));

        creditsMenuButton.onClick.AddListener(ViewTweener.ButtonClickTween(creditsMenuButton,
            creditsMenuClicked,() => ViewManager.Show<CreditsView>()));

        enterUserNameButton.onClick.AddListener(() => ShowUserNamePanel());

        submitUserNameButton.onClick.AddListener(ViewTweener.ButtonClickTween(submitUserNameButton,
            submitUserNameButton.image.sprite,() => SetUsername()));

        AudioController.Instance.PlayAudio(AudioType.MainMenuTheme,true);

        _creditsMenuSprite = creditsMenuButton.image.sprite;

        string userName = GameDataInstance.Instance.UserSave.Name;
        if(string.IsNullOrEmpty(userName)) {
            GetUsername();
        }
        else {
            userNameInput.text = userName;
        }
    }

    public override void Show() {
        base.Show();
        creditsMenuButton.image.sprite = _creditsMenuSprite;
    }

    private void ShowUserNamePanel() {
        userNamePanel.SetActive(true);
    }

    private void HideUserNamePanel() {
        userNamePanel.SetActive(false);
    }

    private void SetUsername() {
        string username = userNameInput.text;

        GameDataInstance.Instance.UserSave.Name = username;
        Debug.Log(GameDataInstance.Instance.UserSave.Name);

        GetUsername();
        HideUserNamePanel();
    }

    private void UpdateUserName() {
        userNameText.SetText(GameDataInstance.Instance.UserSave.Name);
    }

    private void GetUsername() {
        string userName = GameDataInstance.Instance.UserSave.Name;

        if(userName == "") {
            userNameText.SetText("New User");
            ShowUserNamePanel();
        }
        else {
            userNameText.SetText(userName);
        }
    }

    private void Update() {
        UpdateUserName();
    }
}