using System;
using System.Collections.Generic;
using UnityEngine;

public class TeamController : MonoBehaviour
{
    [SerializeField] private CharacterSlot[] characterSlots;
    private CharacterSlotData[] _slotData;
    [SerializeField] private List<CharacterSlotData> team;
    [SerializeField] private int activeSlot;
    
    private bool _activeSlotEmpty;
    
    private int _slotCount;
    private int _filledSlots;

    public static TeamController Instance;

    public static Action<bool> OnTeamReady;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        _slotCount = characterSlots.Length;
    }

    public void SetHeroInSlot(CharacterSlotData heroData)
    {
        Debug.Log($"Setting Slot {activeSlot} to Hero ID {heroData.id}");
        characterSlots[activeSlot].LoadCharacterData(heroData);
        
        CheckSlots();
    }

    private void CheckSlots()
    {
        foreach (var slot in characterSlots)
        {
            if (slot.isEmpty)
            {
                Debug.Log("Team Selection incomplete!");
                OnTeamReady?.Invoke(false);
                return;
            }
        }
        Debug.Log("Team Selection complete! Entering Combat!");
        OnTeamReady?.Invoke(true);
    }
    
    public void SetActiveSlot(int id, bool isEmpty)
    {
        activeSlot = id;
        _activeSlotEmpty = isEmpty;
    }
    
    
}
