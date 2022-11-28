using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckBuilderUIManager : MonoBehaviour
{
    public TMPro.TMP_Dropdown[] characterDropdowns;

    private void Start()
    {
        
    }

    public void DisplayCharacterChoice()
    {
        foreach (var dropdown in characterDropdowns)
        {
            Debug.Log(dropdown.itemText);
            
        }
    }

    public void LoadBattleScene()
    {
        SceneLoader.Load(GameSceneEnum.BattleScene);
    }
    
    public void ReturnToMainScene()
    {
        SceneLoader.Load(GameSceneEnum.MainMenu);
    }
}
