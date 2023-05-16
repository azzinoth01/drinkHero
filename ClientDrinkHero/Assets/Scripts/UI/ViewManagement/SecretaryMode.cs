using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(LoadSprite))]
public class SecretaryMode : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogText;
    private LoadSprite _loadSprite;
    private int currentSelectionIndex = 0;
    private HeroListHandler heroListHandler;
    private void Awake()
    {
        _loadSprite = this.GetComponent<LoadSprite>();
    }

    void Start()
    {
        heroListHandler = new HeroListHandler();
        heroListHandler.LoadingFinished += HeroListHandler_LoadingFinished;
        heroListHandler.RequestData();

    }

    private void HeroListHandler_LoadingFinished()
    {
        Debug.Log(heroListHandler.Heros);
        LoadSecretary(currentSelectionIndex);
    }

    public void LoadSecretary(int increment)
    {
        currentSelectionIndex += increment;
        if (currentSelectionIndex < 0)
            currentSelectionIndex = heroListHandler.Heros.Count - 1;
        if (currentSelectionIndex >= heroListHandler.Heros.Count)
            currentSelectionIndex = 0;

        _loadSprite.SpritePathSufix = "_0.png";
        _loadSprite.LoadNewSprite(heroListHandler.Heros[currentSelectionIndex].SpritePath);
        dialogText.SetText(heroListHandler.Heros[currentSelectionIndex].Name);
    }

    private void Update()
    {
        heroListHandler.Update();
    }
}
