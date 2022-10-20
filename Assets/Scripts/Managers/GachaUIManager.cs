using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaUIManager : MonoBehaviour
{
    public void LoadMainMenuScene()
    {
        SceneLoader.Load(SceneLoader.GameScene.MainMenu);
    }
}
