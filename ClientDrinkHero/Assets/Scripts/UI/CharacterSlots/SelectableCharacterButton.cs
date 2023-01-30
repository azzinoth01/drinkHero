using System;
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
    public int ID => id;
    
    private LoadSprite _loadSprite;
    
    private TeamController _teamController;
    
    private void Awake()
    {
        characterSlotData = new CharacterSlotData();
        _loadSprite = characterPortraitImage.GetComponent<LoadSprite>();
        infoButton.onClick.AddListener(()=> ViewManager.Show<CharacterCardView>());
    }

    public void SetData(CharacterSlotData data)
    {

        characterSlotData = data;
        id = data.id;
        _loadSprite.LoadNewSprite(characterSlotData.characterSpritePath);
        characterName.SetText(characterSlotData.characterName);
        
        MakeSelectable();
    }

    private void MakeSelectable()
    {
        selectButton.onClick.AddListener(()=> TeamController.Instance.SetHeroInSlot(characterSlotData));
        selectButton.onClick.AddListener(()=> Select());
        selectButton.onClick.AddListener(()=> ViewManager.ShowLast());
    }
    
    public void Unlock()
    {
        selectButton.interactable = true;
        lockIcon.enabled = false;
    }
    
    public void Lock()
    {
        selectButton.interactable = false;
        lockIcon.enabled = true;
    }

    private void Select()
    {
        characterSelectedBlock.SetActive(true);
    }

    public void DisableSelection()
    {
        characterSelectedBlock.SetActive(false);
    }
    
    public void EnableSelection()
    {
        characterSelectedBlock.SetActive(true);
    }
}
