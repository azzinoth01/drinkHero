using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenuView : View
{
    [Header("Ui Buttons")] [SerializeField]
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
    
    public override void Initialize()
    {
        battlePreparationMenuButton.onClick.AddListener(ViewTweener.ButtonClickTween(battlePreparationMenuButton, 
            battlePreparationClicked, () => SceneLoader.Load(GameSceneEnum.BattlePreparationMenu)));
        
        gachaMenuButton.onClick.AddListener(ViewTweener.ButtonClickTween(gachaMenuButton, 
            gachaMenuClicked, () => SceneLoader.Load(GameSceneEnum.GachaMenuScene)));

        optionsMenuButton.onClick.AddListener(ViewTweener.ButtonClickTween(optionsMenuButton, 
            optionsMenuButton.image.sprite, () => ViewManager.Show<OptionsMenuView>()));
        
        creditsMenuButton.onClick.AddListener(ViewTweener.ButtonClickTween(creditsMenuButton, 
            creditsMenuClicked, () => ViewManager.Show<CreditsView>()));

        enterUserNameButton.onClick.AddListener(() => ShowUserNamePanel());
        
        submitUserNameButton.onClick.AddListener(ViewTweener.ButtonClickTween(submitUserNameButton, 
            submitUserNameButton.image.sprite, () => SetUsername()));

        AudioController.Instance.PlayAudio(AudioType.MainMenuTheme, true);

        _creditsMenuSprite = creditsMenuButton.image.sprite;

        //GetUsername();
    }

    public override void Show()
    {
        base.Show();
        creditsMenuButton.image.sprite = _creditsMenuSprite;
    }

    private void ShowUserNamePanel()
    {
        userNamePanel.SetActive(true);
    }
    
    private void HideUserNamePanel()
    {
        userNamePanel.SetActive(false);
    }

    private void SetUsername()
    {
        string username = userNameInput.text;
        string request = ClientFunctions.RenameUser(username);
        
        UserSingelton.Instance.UserObject.UpdateUserDataRequest(request);
        
        Debug.Log(UserSingelton.Instance.UserObject.User.Name);
        
        GetUsername();
        HideUserNamePanel();
    }

    private void UpdateUserName()
    {
        userNameText.SetText(UserSingelton.Instance.UserObject.User.Name);
    }
    
    private void GetUsername()
    {
        string userName = UserSingelton.Instance.UserObject.User.Name;

        if (userName == "")
        {
            userNameText.SetText("New User");
            ShowUserNamePanel();
        }
        else
        {
            userNameText.SetText(userName);
        }
    }

    private void Update()
    {
        UpdateUserName();
    }
}