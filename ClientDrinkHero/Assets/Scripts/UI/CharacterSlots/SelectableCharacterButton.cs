using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectableCharacterButton : MonoBehaviour
{
    private CharacterSlotData _characterSlotData;
    private TextMeshProUGUI _characterName;
    private Image _characterFactionImage;
    private Image _characterPortraitImage;

    public void SetData()
    {
        _characterName.SetText(_characterSlotData.characterName);
        _characterPortraitImage.sprite = _characterSlotData.characterPortrait;
        _characterFactionImage.sprite = _characterSlotData.characterFaction;
    }
}
