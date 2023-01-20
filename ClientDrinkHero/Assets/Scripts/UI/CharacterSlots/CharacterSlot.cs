using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour
{
    [Header("Character Data")] 
    [SerializeField] private int slotID;
    [SerializeField] private CharacterSlotData slotData;
    
    [SerializeField] private Image characterPortrait;
    [SerializeField] private TextMeshProUGUI characterName;
    
    private Button _slotButton;
    public bool isEmpty;
    private void Awake()
    {
        _slotButton = GetComponent<Button>();
        _slotButton.onClick.AddListener(() => TeamController.Instance.SetActiveSlot(slotID, isEmpty));
    }
    
    public void LoadCharacterData(CharacterSlotData data)
    {
        if (data == null)
        {
            Debug.Log("Data is null!");
            return;
        }
        
        slotData = data;

        characterPortrait.sprite = slotData.characterPortrait.sprite;
        characterPortrait.enabled = true;

        characterName.SetText(slotData.characterName);
        characterName.enabled = true;
        
        isEmpty = false;
    }
    
    public void ClearCharacterData()
    {
        slotData = null;
        
        characterPortrait = null;
        characterPortrait.enabled = false;

        characterName.SetText("");
        characterName.enabled = false;

        isEmpty = true;
    }

    public void TransmitCharacterData()
    {
        
    }
}
