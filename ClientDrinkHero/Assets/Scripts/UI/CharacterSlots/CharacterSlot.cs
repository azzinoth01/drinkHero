using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour
{
    [Header("Character Data")] 
    [SerializeField] private int slotID;
    [SerializeField] public CharacterSlotData slotData;
    [SerializeField] private Image characterPortrait;
    [SerializeField] private TextMeshProUGUI characterName;

    private int _lastHeroIdInSlot;
    public bool IsEmpty { get; set; }
    
    private LoadSprite _loadSprite;
    private Button _slotButton;

    public static Action<int> OnCharacterDeselect;

    private void Awake()
    {
        IsEmpty = true;
        _loadSprite = GetComponent<LoadSprite>();
        _slotButton = GetComponent<Button>();
        _slotButton.onClick.AddListener(() => TeamController.Instance.SetActiveSlot(slotID, IsEmpty));
    }

    public void LoadCharacterData(CharacterSlotData data)
    {
        if (!IsEmpty)
        {
            //Tell CharacterSelectView to deselect lastHeroId
            Debug.Log($"<color=red>Re-Enabling Hero ID {_lastHeroIdInSlot}</color>");
            OnCharacterDeselect?.Invoke(_lastHeroIdInSlot);
        }
            
        slotData = data;

        _loadSprite.LoadNewSprite(data.characterSpritePath);
        characterPortrait.enabled = true;

        characterName.SetText(slotData.characterName);
        characterName.enabled = true;

        _lastHeroIdInSlot = slotData.id;
        
        IsEmpty = false;
    }

    public void ClearCharacterData()
    {
        slotData = new CharacterSlotData();
        
        characterPortrait.enabled = false;

        characterName.SetText("");
        characterName.enabled = false;

        IsEmpty = true;
    }
}