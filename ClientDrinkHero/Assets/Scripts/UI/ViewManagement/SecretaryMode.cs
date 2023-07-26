using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SecretaryMode : MonoBehaviour
{
    [SerializeField] private List<Hero> unlockedHeroes = new List<Hero>();
    [SerializeField] private GameObject dialogTextBox;
    [SerializeField] private GameObject buttons;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private Transform secretaryHolder;
    [SerializeField] private float dialogTime = 5f;
    private int currentSelectionIndex = 0;
    private GameObject currentSecretary;
    private float textBoxTimer;
    private int lastQuoteIndex = -1;

    private UnlockedHeroesPreviewHandler _unlockedHeroesPreviewHandler;

    void Start()
    {
        buttons.SetActive(false);
        DisableTextBox();

        foreach (Hero hero in HeroHolder.Instance.Heroes)
        {
            if (hero.Unlocked)
            {
                unlockedHeroes.Add(hero);
            }
        }
        LoadSecretary(currentSelectionIndex);

        buttons.SetActive(true);
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
        int randomQuoteIndex = Random.Range(0, unlockedHeroes[currentSelectionIndex].SecretaryQuotes.Count);
        if (lastQuoteIndex == randomQuoteIndex)
        {
            randomQuoteIndex = randomQuoteIndex == 0 ? ++randomQuoteIndex : --randomQuoteIndex;
        }
        lastQuoteIndex = randomQuoteIndex;

        SetText(unlockedHeroes[currentSelectionIndex].SecretaryQuotes[randomQuoteIndex]);
        string heroName = unlockedHeroes[currentSelectionIndex].Name;
        currentSecretary.GetComponentInChildren<Image>().sprite = AssetLoader.Instance.BorrowSprite("Assets/Art/Character/" + heroName + "/"  + heroName + "_" + randomQuoteIndex + ".png");
    }

    public void TouchSpecial()
    {
        currentSecretary.GetComponentInChildren<Image>().sprite = unlockedHeroes[currentSelectionIndex].SecretarySpezialTouchImage;
        SetText(unlockedHeroes[currentSelectionIndex].SecretarySpezialTouchQuote);
        lastQuoteIndex = -1;
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