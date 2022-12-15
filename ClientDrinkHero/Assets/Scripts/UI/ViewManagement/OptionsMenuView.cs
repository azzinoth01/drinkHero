using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuView : View
{
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button returnButton;

    public override void Initialize()
    {
        returnButton.onClick.AddListener(() => ViewManager.ShowLast());
    }
}
