using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTeamSelection : MonoBehaviour
{
    [SerializeField] private GameObject[] characterSlots;
    [SerializeField] private List<CharacterSlotData> characterSlotData;
    private int _slotCount;

    private void Awake()
    {
        _slotCount = characterSlots.Length;
    }

    private void UpdateSlots()
    {
        List<CharacterSlotData> data = new List<CharacterSlotData>();
        
        foreach (var slot in characterSlots)
        {
            var characterSlot = slot.GetComponent<CharacterSlot>();
            var slotData = characterSlot.SlotData;
            
            if(slotData!=null)
            {
                characterSlot.isEmpty = false;
                data.Add(slotData);
            }
            else
            {
                characterSlot.isEmpty = true;
                data.Add(new CharacterSlotData());
            }
        }

        characterSlotData = data;
    }

    public bool TeamSelectionComplete()
    {
        foreach (var slot in characterSlots)
        {
            var characterSlot = slot.GetComponent<CharacterSlot>();
            var slotData = characterSlot.SlotData;

            if(characterSlot.isEmpty)
            {
                Debug.Log("Team Selection incomplete!");
                return false;
            }
        }

        Debug.Log("Team Selection complete! Entering Combat!");
        return true;
    }
}
