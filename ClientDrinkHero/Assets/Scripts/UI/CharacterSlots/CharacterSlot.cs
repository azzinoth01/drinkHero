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

    public bool IsEmpty { get; set; }
    
    private LoadSprite _loadSprite;
    private Button _slotButton;

    private void Awake()
    {
        _loadSprite = GetComponent<LoadSprite>();
        _slotButton = GetComponent<Button>();
        _slotButton.onClick.AddListener(() => TeamController.Instance.SetActiveSlot(slotID, IsEmpty));
    }

    public void LoadCharacterData(CharacterSlotData data)
    {
        slotData = data;

        _loadSprite.LoadNewSprite(data.characterSpritePath);
        characterPortrait.enabled = true;

        characterName.SetText(slotData.characterName);
        characterName.enabled = true;

        IsEmpty = false;
    }

    public void ClearCharacterData()
    {
        slotData = null;

        characterPortrait = null;
        characterPortrait.enabled = false;

        characterName.SetText("");
        characterName.enabled = false;

        IsEmpty = true;
    }
}