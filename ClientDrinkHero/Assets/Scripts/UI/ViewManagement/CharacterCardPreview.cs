using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCardPreview : MonoBehaviour
{
    [SerializeField] private int cost;
    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private TextMeshProUGUI cardDescription;
    [SerializeField] private Image cardPortrait;

    public void SetData(CardData data)
    {
        cost = data.cost;
        cardName.SetText(data.name);
        cardDescription.SetText(data.description);
        cardPortrait = data.portrait;
    }
}