using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SecretaryMode : MonoBehaviour
{
    [SerializeField] private List<Hero> heroes = new List<Hero>();
    [SerializeField] private List<Hero> unlockedHeroes = new List<Hero>();
    [SerializeField] private GameObject dialogTextBox;
    [SerializeField] private GameObject buttons;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private Transform secretaryHolder;
    [SerializeField] private float dialogTime = 5f;
    private int currentSelectionIndex = 0;
    private GameObject currentSecretary;
    private float textBoxTimer;

    private UnlockedHeroesPreviewHandler _unlockedHeroesPreviewHandler;

    void Start()
    {
        _unlockedHeroesPreviewHandler = new UnlockedHeroesPreviewHandler();

        _unlockedHeroesPreviewHandler.LoadingFinished += _unlockedHeroesPreviewHandler_LoadingFinished;
        _unlockedHeroesPreviewHandler.RequestData();

        buttons.SetActive(false);
        DisableTextBox();
    }

    private void _unlockedHeroesPreviewHandler_LoadingFinished()
    {
        buttons.SetActive(true);
        foreach (HeroToUserDatabase dbHero in _unlockedHeroesPreviewHandler.UnlockedHeros)
        {
            Hero newHero = heroes.FirstOrDefault(x => x.ID == dbHero.Hero.Id);
            if(newHero != null)
                unlockedHeroes.Add(newHero);
        }
        LoadSecretary(currentSelectionIndex);
    }

    public void LoadSecretary(int increment)
    {
        currentSelectionIndex += increment;
        if (currentSelectionIndex < 0)
            currentSelectionIndex = unlockedHeroes.Count - 1;
        if (currentSelectionIndex >= unlockedHeroes.Count)
            currentSelectionIndex = 0;

        if (currentSecretary)
            Destroy(currentSecretary);

        currentSecretary = Instantiate(unlockedHeroes[currentSelectionIndex].SecretaryPrefab, secretaryHolder);
        currentSecretary.GetComponent<Button>().onClick.AddListener(Touch);
        currentSecretary.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(TouchSpecial);
        DisableTextBox();
    }
    public void Touch()
    {
        SetText(unlockedHeroes[currentSelectionIndex].SecretaryQuotes[Random.Range(0, unlockedHeroes[currentSelectionIndex].SecretaryQuotes.Count)]);
    }

    public void TouchSpecial()
    {
        currentSecretary.GetComponentInChildren<Image>().sprite = unlockedHeroes[currentSelectionIndex].SecretarySpezialTouchImage;
        SetText(unlockedHeroes[currentSelectionIndex].SecretarySpezialTouchQuote);
    }

    private void SetText(string text)
    {
        dialogTextBox.SetActive(true);
        dialogText.text = text;
        textBoxTimer = 0;
    }

    private void DisableTextBox()
    {
        dialogTextBox.SetActive(false);
        dialogText.text = "";
    }

    private void Update()
    {
        _unlockedHeroesPreviewHandler.Update();
        if (textBoxTimer < dialogTime)
        {
            textBoxTimer += Time.deltaTime;
        }
        else
        {
            DisableTextBox();
        }
    }
}