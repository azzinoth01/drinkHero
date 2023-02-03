using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectableCharacterButton : MonoBehaviour
{
    [SerializeField] private GameObject characterSelectedBlock;
    [SerializeField] private CharacterSlotData characterSlotData;
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private Image characterFactionImage;
    [SerializeField] private Image characterPortraitImage;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private Button selectButton;
    [SerializeField] private Button deSelectButton;
    [SerializeField] private Button infoButton;
    [SerializeField] private int id;

    [SerializeField] private CharacterCardView characterCardView;

    private LoadSprite _loadSprite;
    private TeamController _teamController;

    private bool _alreadyInParty;
    public int ID => id;

    public static Action<int> OnClearSlot;

    private void Awake()
    {
        characterSlotData = new CharacterSlotData();
        _loadSprite = characterPortraitImage.GetComponent<LoadSprite>();
        
        infoButton.onClick.AddListener(() => ViewManager.Show<CharacterCardView>());
        infoButton.onClick.AddListener(() => characterCardView.LoadCharacterData(id));
    }

    private void Start()
    {
        if (!characterCardView)
        {
            characterCardView = ViewManager.Instance.GetView<CharacterCardView>();
        }
    }

    public void SetData(CharacterSlotData data)
    {
        characterSlotData = data;
        id = data.id;
        _loadSprite.LoadNewSprite(characterSlotData.characterSpritePath);
        characterName.SetText(characterSlotData.characterName);
        
        Initialize();
    }

    private void Initialize()
    {
        selectButton.onClick.AddListener(() => TeamController.Instance.SetHeroInSlot(characterSlotData));
        selectButton.onClick.AddListener(() => SelectCharacter());
        selectButton.onClick.AddListener(() => ViewManager.ShowLast());
        selectButton.onClick.AddListener(() => AudioController.Instance.PlayAudio(AudioType.SFXButtonYes));;
        
        deSelectButton.onClick.AddListener(() => DeSelectCharacter());
        deSelectButton.onClick.AddListener(() => ViewManager.ShowLast());
        deSelectButton.onClick.AddListener(() => AudioController.Instance.PlayAudio(AudioType.SFXButtonNo));
        
        DeSelectCharacter();
    }

    public void Unlock()
    {
        selectButton.interactable = true;
        infoButton.interactable = true;
        lockIcon.SetActive(false);
    }

    public void Lock()
    {
        selectButton.interactable = false;
        infoButton.interactable = false;
        lockIcon.SetActive(true);
    }

    public void CheckIfSelected()
    {
        if (_alreadyInParty)
        {
            DeSelectCharacter();
            return;
        }
        SelectCharacter();
    }
    
    private void SelectCharacter()
    {
        characterSelectedBlock.SetActive(true);
        _alreadyInParty = true;
    }

    private void DeSelectCharacter()
    {
        Debug.Log($"<color=green>Re-Enabling Hero ID {characterSlotData.characterName}</color>");
        characterSelectedBlock.SetActive(false);
        _alreadyInParty = false;

        OnClearSlot?.Invoke(characterSlotData.id);
    }
}