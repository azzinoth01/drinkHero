using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCardView : View
{
    [SerializeField] private TextMeshProUGUI characterNameLabel;
    [SerializeField] private Image characterFactionImage;
    [SerializeField] private Image characterPortraitImage;
    [SerializeField] private CharacterCardPreview[] cardPreviews;
    [SerializeField] private Button backButton;
    public override void Initialize()
    {
        backButton.onClick.AddListener(() => ViewManager.ShowLast());
    }

    private void LoadCharacterData(int id)
    {
        
    }

    public override void Show()
    {
        base.Show();
        //LoadCharacterData();
    }
}
