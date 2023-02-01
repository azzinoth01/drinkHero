using UnityEngine;
using UnityEngine.UI;

public class DeckMenuView : View
{
    [SerializeField] private Button backButton;

    public override void Initialize()
    {
        backButton.onClick.AddListener(() => ViewManager.ShowLast());
    }
}