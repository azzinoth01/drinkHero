using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour
{
    [field: Header("Character Data")]
    [field: SerializeField]
    public CharacterSlotData SlotData { get; }

    private Button _slotButton;
    private Image _characterPortrait;
    private Image _characterFaction;
    private TextMeshProUGUI _characterName;
    
    public bool isEmpty;

    private void Awake()
    {
        
    }
    
    public void LoadCharacterData()
    {
        
    }

    public void TransmitCharacterData()
    {
        
    }
}
