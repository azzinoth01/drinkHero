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
    [SerializeField] private bool isUnlocked;
    
    private TeamController _teamController;
    
    private void Awake()
    {
        SetData();
        
        selectButton.onClick.AddListener(()=> TeamController.Instance.SetHeroInSlot(characterSlotData));
        selectButton.onClick.AddListener(()=> Select());
        selectButton.onClick.AddListener(()=> ViewManager.ShowLast());

        if (isUnlocked)
        {
            selectButton.interactable = true;
            lockIcon.enabled = false;
        }
        else
        {
            selectButton.interactable = false;
            lockIcon.enabled = true;
        }
    }
    
    public void SetData()
    {
        characterSlotData.characterName = characterName.text;
        characterSlotData.characterPortrait = characterPortraitImage;
        characterSlotData.characterFaction = characterFactionImage;
        // id
        // rank?
        // isUnlocked?
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
