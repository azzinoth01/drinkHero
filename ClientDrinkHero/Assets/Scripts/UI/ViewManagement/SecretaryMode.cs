using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SecretaryMode : MonoBehaviour
{
    [SerializeField] private GameObject dialogTextBox;
    [SerializeField] private GameObject buttons;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private Transform secretaryHolder;
    [SerializeField] private float dialogTime = 5f;
    private int currentSelectionIndex = 0;
    private GameObject currentSecretary;
    private float textBoxTimer;
    private int lastQuoteIndex = -1;

    void Start()
    {
        buttons.SetActive(false);
        DisableTextBox();

        LoadSecretary(currentSelectionIndex);

        buttons.SetActive(true);
    }

    public void LoadSecretary(int increment)
    {
        currentSelectionIndex += increment;
        if (currentSelectionIndex < 0)
            currentSelectionIndex = HeroHolder.Instance.Heroes.Count - 1;
        if (currentSelectionIndex >= HeroHolder.Instance.Heroes.Count)
            currentSelectionIndex = 0;

        if (currentSecretary)
            Destroy(currentSecretary);

        currentSecretary = Instantiate(HeroHolder.Instance.Heroes[currentSelectionIndex].SecretaryPrefab, secretaryHolder);
        currentSecretary.GetComponent<Button>().onClick.AddListener(Touch);
        currentSecretary.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(TouchSpecial);
        DisableTextBox();
    }
    public void Touch()
    {
        int randomQuoteIndex = Random.Range(0, HeroHolder.Instance.Heroes[currentSelectionIndex].SecretaryQuotes.Count);
        if (lastQuoteIndex == randomQuoteIndex)
        {
            randomQuoteIndex = randomQuoteIndex == 0 ? ++randomQuoteIndex : --randomQuoteIndex;
        }
        lastQuoteIndex = randomQuoteIndex;

        SetText(HeroHolder.Instance.Heroes[currentSelectionIndex].SecretaryQuotes[randomQuoteIndex]);
        string heroName = HeroHolder.Instance.Heroes[currentSelectionIndex].Name;
        currentSecretary.GetComponentInChildren<Image>().sprite = AssetLoader.Instance.BorrowSprite("Assets/Art/Character/" + heroName + "/"  + heroName + "_" + randomQuoteIndex + ".png");
    }

    public void TouchSpecial()
    {
        currentSecretary.GetComponentInChildren<Image>().sprite = HeroHolder.Instance.Heroes[currentSelectionIndex].SecretarySpezialTouchImage;
        SetText(HeroHolder.Instance.Heroes[currentSelectionIndex].SecretarySpezialTouchQuote);
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