using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SecretaryMode : MonoBehaviour
{
    [SerializeField] private List<Hero> heroes = new List<Hero>();
    [SerializeField] private GameObject dialogTextBox;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private Transform secretaryHolder;
    [SerializeField] private float dialogTime = 5f;
    private int currentSelectionIndex = 0;
    private GameObject currentSecretary;
    private float textBoxTimer;

    void Start()
    {
        LoadSecretary(currentSelectionIndex);
    }

    public void LoadSecretary(int increment)
    {
        currentSelectionIndex += increment;
        if (currentSelectionIndex < 0)
            currentSelectionIndex = heroes.Count - 1;
        if (currentSelectionIndex >= heroes.Count)
            currentSelectionIndex = 0;

        if (currentSecretary)
            Destroy(currentSecretary);

        currentSecretary = Instantiate(heroes[currentSelectionIndex].SecretaryPrefab, secretaryHolder);
        currentSecretary.GetComponentInChildren<Button>().onClick.AddListener(TouchSpecial);

        SetText(heroes[currentSelectionIndex].SecretaryQuotes[Random.Range(0, heroes[currentSelectionIndex].SecretaryQuotes.Count)]);
    }

    public void TouchSpecial()
    {
        currentSecretary.GetComponentInChildren<Image>().sprite = heroes[currentSelectionIndex].SecretarySpezialTouchImage;
        SetText(heroes[currentSelectionIndex].SecretarySpezialTouchQuote);
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