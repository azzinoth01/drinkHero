using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectView : View
{
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject[] characterButtonObjects;
    [SerializeField] private SelectableCharacterButton[] characterButtons;
    public override void Initialize()
    {
        backButton.onClick.AddListener(() => ViewManager.ShowLast());
        PopulateCharacterList();
    }

    private void PopulateCharacterList()
    {
        List<SelectableCharacterButton> buttons = new List<SelectableCharacterButton>();
        
        foreach (var character in characterButtonObjects)
        {
            character.SetActive(true);
            var btn = character.GetComponent<SelectableCharacterButton>();
            buttons.Add(btn);
        }

        characterButtons = buttons.ToArray();
    }

    private void DisableCharacter(int id)
    {
        characterButtons[id-1].DisableSelection();
    }
    
    private void EnableCharacter(int id)
    {
        characterButtons[id-1].EnableSelection();
    }
}
