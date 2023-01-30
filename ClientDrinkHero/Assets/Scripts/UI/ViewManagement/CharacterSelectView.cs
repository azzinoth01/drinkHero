using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectView : View
{
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject[] characterButtonObjects;
    [SerializeField] private SelectableCharacterButton[] characterButtons;

    private AllHerosPreviewHandler _allHerosPreviewHandler;
    private UnlockedHerosPreviewHandler _unlockedHerosPreviewHandler;

    private int _listCounter;
    
    public override void Initialize()
    {
        backButton.onClick.AddListener(() => ViewManager.ShowLast());
        
        _allHerosPreviewHandler = new AllHerosPreviewHandler();
        _unlockedHerosPreviewHandler = new UnlockedHerosPreviewHandler();
        
        _allHerosPreviewHandler.LoadingFinished += CheckLists;
        _unlockedHerosPreviewHandler.LoadingFinished += CheckLists;
    }

    private void PopulateCharacterList()
    {
        List<HeroDatabase> allHeros = _allHerosPreviewHandler.Heros;
        List<HeroToUserDatabase> unlockedHeros = _unlockedHerosPreviewHandler.UnlockedHeros;
        List<SelectableCharacterButton> buttons = new List<SelectableCharacterButton>();

        for (int i = 0; i < characterButtonObjects.Length; i++)
        {
            characterButtonObjects[i].SetActive(true);
            
            var btn = characterButtonObjects[i].GetComponent<SelectableCharacterButton>();
            btn.Lock();
            buttons.Add(btn);
        }

        for (int i = 0; i < allHeros.Count; i++)
        {
            var data = new CharacterSlotData();
            data.id = allHeros[i].Id;
            data.characterName = allHeros[i].Name;
            data.characterSpritePath = allHeros[i].SpritePath;
            
            buttons[i].SetData(data);
            Debug.Log($"<color=red>Selectable Character Button {i}</color> set to id:{data.id}, SpritePath:{data.characterSpritePath}, id:{data.characterName}!");
        }
        
        foreach (var unlockedHero in unlockedHeros)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                if(buttons[i].ID == unlockedHero.RefHero) buttons[i].Unlock();
            }
        }

        characterButtons = buttons.ToArray();
    }

    private void Update()
    {
        _allHerosPreviewHandler.Update();
        _unlockedHerosPreviewHandler.Update();
    }

    private void CheckLists()
    {
        _listCounter += 1;
        if (_listCounter == 2)
        {
            PopulateCharacterList();
        }
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
