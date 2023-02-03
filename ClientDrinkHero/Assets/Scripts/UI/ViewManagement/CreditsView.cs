using UnityEngine;
using UnityEngine.UI;

public class CreditsView : View
{
    [SerializeField] private Button resumeButton;

    public override void Initialize()
    {
        resumeButton.onClick.AddListener(ViewTweener.ButtonClickTween(resumeButton, 
            resumeButton.image.sprite, () => ViewManager.ShowLast()));
    }
}
