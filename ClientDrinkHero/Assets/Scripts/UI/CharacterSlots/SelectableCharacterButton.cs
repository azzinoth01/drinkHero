using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectableCharacterButton : MonoBehaviour
{
    private CharacterSlotData _characterSlotData;
    private TextMeshProUGUI _characterName;
    private Image _characterFactionImage;
    private Image _characterPortraitImage;

    [SerializeField] private bool _isUnlocked;
    private Button _selectButton;
    private void Awake()
    {
        _selectButton = GetComponent<Button>();
        if (!_isUnlocked) _selectButton.interactable = false;
    }

    public void SetData()
    {
        _characterName.SetText(_characterSlotData.characterName);
        _characterPortraitImage.sprite = _characterSlotData.characterPortrait;
        _characterFactionImage.sprite = _characterSlotData.characterFaction;
        // id
        // rank?
        // isUnlocked?
    }
}
