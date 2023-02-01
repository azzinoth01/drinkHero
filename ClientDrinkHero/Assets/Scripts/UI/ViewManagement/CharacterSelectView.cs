using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectView : View
{
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject[] characterButtonObjects;
    [SerializeField] private SelectableCharacterButton[] characterButtons;
    [SerializeField] private GameObject loadingPanel;

    private AllHeroesPreviewHandler _allHeroesPreviewHandler;
    private UnlockedHeroesPreviewHandler _unlockedHeroesPreviewHandler;

    private List<HeroDatabase> _allHeroes;
    public static List<HeroToUserDatabase> UnlockedHeroes;

    private int _listCounter;

    public override void Initialize()
    {
        CharacterSlot.OnCharacterDeselect += EnableCharacter;
        
        backButton.onClick.AddListener(() => ViewManager.ShowLast());

        UnlockedHeroes = new List<HeroToUserDatabase>();
        _allHeroes = new List<HeroDatabase>();

        _allHeroesPreviewHandler = new AllHeroesPreviewHandler();
        _unlockedHeroesPreviewHandler = new UnlockedHeroesPreviewHandler();

        _allHeroesPreviewHandler.LoadingFinished += CheckLists;
        _unlockedHeroesPreviewHandler.LoadingFinished += CheckLists;

        _allHeroesPreviewHandler.RequestData();
        _unlockedHeroesPreviewHandler.RequestData();

        ShowLoadingPanel();
    }

    private void OnDestroy()
    {
        CharacterSlot.OnCharacterDeselect -= EnableCharacter;
        _allHeroesPreviewHandler.LoadingFinished -= CheckLists;
        _unlockedHeroesPreviewHandler.LoadingFinished -= CheckLists;
    }

    private void PopulateCharacterList()
    {
        if (_allHeroes.Count == 0) _allHeroes = _allHeroesPreviewHandler.Heros;

        UnlockedHeroes = _unlockedHeroesPreviewHandler.UnlockedHeros;

        var selectableCharacterButtons = new List<SelectableCharacterButton>();

        for (var i = 0; i < characterButtonObjects.Length; i++)
        {
            characterButtonObjects[i].SetActive(true);
            var btn = characterButtonObjects[i].GetComponent<SelectableCharacterButton>();
            btn.Lock();
            selectableCharacterButtons.Add(btn);
        }

        for (var i = 0; i < _allHeroes.Count; i++)
        {
            var data = new CharacterSlotData();
            data.id = _allHeroes[i].Id;
            data.characterName = _allHeroes[i].Name;
            data.characterSpritePath = _allHeroes[i].SpritePath;
            selectableCharacterButtons[i].SetData(data);
        }

        foreach (var unlockedHero in UnlockedHeroes)
            for (var i = 0; i < selectableCharacterButtons.Count; i++)
                if (selectableCharacterButtons[i].ID == unlockedHero.RefHero)
                    selectableCharacterButtons[i].Unlock();

        characterButtons = selectableCharacterButtons.ToArray();
    }

    private void Update()
    {
        _allHeroesPreviewHandler.Update();
        _unlockedHeroesPreviewHandler.Update();
    }

    private void CheckLists()
    {
        _listCounter += 1;
        if (_listCounter == 2)
        {
            PopulateCharacterList();
            HideLoadingPanel();
        }
    }
    
    private void EnableCharacter(int id)
    {
        Debug.Log($"<color=red>Attempting to (re-)enable {id - 1}</color>");
        characterButtons[id - 1].CheckIfSelected();
    }

    private void ShowLoadingPanel()
    {
        loadingPanel.SetActive(true);
    }

    private void HideLoadingPanel()
    {
        loadingPanel.SetActive(false);
    }
}