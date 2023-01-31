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
    [SerializeField] private Image lockIcon;
    [SerializeField] private Button selectButton;
    [SerializeField] private Button infoButton;
    [SerializeField] private int id;

    [SerializeField] private CharacterCardView characterCardView;

    private LoadSprite _loadSprite;
    private TeamController _teamController;
    public int ID => id;

    private void Awake()
    {
        characterSlotData = new CharacterSlotData();
        _loadSprite = characterPortraitImage.GetComponent<LoadSprite>();
        characterCardView = ViewManager.Instance.GetView<CharacterCardView>();
        infoButton.onClick.AddListener(() => ViewManager.Show<CharacterCardView>());
        infoButton.onClick.AddListener(() => characterCardView.LoadCharacterData(id));
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
        selectButton.onClick.AddListener(() => DisableSelection());
        selectButton.onClick.AddListener(() => ViewManager.ShowLast());
        EnableSelection();
    }

    public void Unlock()
    {
        selectButton.interactable = true;
        infoButton.interactable = true;
        lockIcon.enabled = false;
    }

    public void Lock()
    {
        selectButton.interactable = false;
        infoButton.interactable = false;
        lockIcon.enabled = true;
    }

    public void DisableSelection()
    {
        characterSelectedBlock.SetActive(true);
    }

    public void EnableSelection()
    {
        Debug.Log($"<color=green>Re-Enabling Hero ID {characterSlotData.characterName}</color>");
        characterSelectedBlock.SetActive(false);
    }
}