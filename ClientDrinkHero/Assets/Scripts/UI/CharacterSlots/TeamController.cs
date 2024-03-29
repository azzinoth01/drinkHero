using System;
using UnityEngine;

public class TeamController : MonoBehaviour
{
    public static TeamController Instance;

    private int[] CurrentTeamIds;

    public static Action<bool> OnTeamReady;
    [SerializeField] private CharacterSlot[] characterSlots;
    [SerializeField] private int activeSlot;

    private bool _activeSlotEmpty;
    private int _filledSlots;

    private int _slotCount;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        _slotCount = characterSlots.Length;

        CurrentTeamIds = new[] { 0, 0, 0, 0 };

        SelectableCharacterButton.OnClearSlot += ClearSlot;
    }

    private void OnDestroy()
    {
        SelectableCharacterButton.OnClearSlot -= ClearSlot;
    }

    public void SetHeroInSlot(CharacterSlotData heroData)
    {
        characterSlots[activeSlot].LoadCharacterData(heroData);
        CheckSlots();
    }

    public void CheckSlots()
    {
        foreach (var slot in characterSlots)
            if (slot.IsEmpty)
            {
                Debug.Log("Team Selection incomplete!");
                OnTeamReady?.Invoke(false);
                return;
            }

        for (var i = 0; i < characterSlots.Length; i++)
        {
            CurrentTeamIds[i] = characterSlots[i].slotData.id;
            UIDataContainer.TeamIds = CurrentTeamIds;
        }
        OnTeamReady?.Invoke(true);
    }

    public void SetActiveSlot(int id, bool isEmpty)
    {
        activeSlot = id;
        _activeSlotEmpty = isEmpty;
    }

    private void ClearSlot(int id)
    {
        foreach (var slot in characterSlots)
        {
            if(id == slot.slotData.id) slot.ClearCharacterData();
        }
    }
}