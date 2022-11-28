using UnityEngine;
using UnityEngine.UI;

public class LoadingProgressBar : MonoBehaviour
{
    private Image _loadingProgressBar;

    private void Awake()
    {
        _loadingProgressBar = GetComponent<Image>();
    }
    
    private void Update()
    {
        if (_loadingProgressBar.fillAmount < SceneLoader.GetLoadingProgress())
        {
            _loadingProgressBar.fillAmount += 0.1f; 
        }
    }
}
