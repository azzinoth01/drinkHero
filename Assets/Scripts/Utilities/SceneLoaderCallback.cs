using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoaderCallback : MonoBehaviour
{
    private bool _isFirstUpdate = true;
    
    private void Update()
    {
        // TODO: explain
        if (_isFirstUpdate)
        {
            _isFirstUpdate = false;
            SceneLoader.LoaderCallback();
        }
    }
}
