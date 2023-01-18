using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectView : View
{
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject[] selectableCharacters;
    public override void Initialize()
    {
        backButton.onClick.AddListener(() => ViewManager.ShowLast());
        PopulateCharacterList();
    }

    private void PopulateCharacterList()
    {
        foreach (var character in selectableCharacters)
        {
            character.SetActive(true);
        }
    }
}
