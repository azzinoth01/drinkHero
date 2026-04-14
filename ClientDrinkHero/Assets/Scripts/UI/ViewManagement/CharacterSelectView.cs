using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectView : View
{
    [SerializeField] private Button backButton;
    [SerializeField] private Sprite backButtonClicked;
    private Sprite _backButtonInitial;

    [SerializeField] private GameObject[] characterButtonObjects;
    [SerializeField] private SelectableCharacterButton[] characterButtons;


    private int _listCounter;

    public override void Initialize() {

        CharacterSlot.OnCharacterDeselect += EnableCharacter;

        backButton.onClick.AddListener(ViewTweener.ButtonClickTween(backButton,backButtonClicked,() => ViewManager.ShowLast()));

        _backButtonInitial = backButton.image.sprite;

        PopulateCharacterList();
    }

    private void OnDestroy() {
        CharacterSlot.OnCharacterDeselect -= EnableCharacter;
    }

    private void PopulateCharacterList() {

        var selectableCharacterButtons = new List<SelectableCharacterButton>();
        for(int i = 0; i < characterButtonObjects.Length; i++) {
            characterButtonObjects[i].SetActive(true);
            SelectableCharacterButton characterButton = characterButtonObjects[i].GetComponent<SelectableCharacterButton>();
            characterButton.Lock();
            selectableCharacterButtons.Add(characterButton);
        }

        List<HeroData> allHeroes = GameDataInstance.Instance.GameDataDatabase.HeroData;
        for(int i = 0; i < allHeroes.Count; i++) {
            CharacterSlotData data = new CharacterSlotData();
            data.id = allHeroes[i].Id;
            data.characterName = allHeroes[i].Name;
            data.characterSpritePath = allHeroes[i].SpritePath;
            selectableCharacterButtons[i].SetData(data);
        }

        List<HeroObject> unlockedHeroes = GameDataInstance.Instance.OwnedHeroes;
        foreach(HeroObject unlockedHero in unlockedHeroes) {
            for(int i = 0; i < selectableCharacterButtons.Count; i++) {
                if(selectableCharacterButtons[i].ID == unlockedHero.Id) {
                    selectableCharacterButtons[i].Unlock();
                }
            }
        }
        characterButtons = selectableCharacterButtons.ToArray();
    }

    private void EnableCharacter(string id) {
        //Debug.Log($"<color=red>Attempting to (re-)enable {id - 1}</color>");
        //characterButtons[id - 1].CheckIfSelected();
    }

    public override void Show() {
        base.Show();
        backButton.image.sprite = _backButtonInitial;
    }
}