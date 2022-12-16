using UnityEngine;

public interface ICardDisplay {

    public string CostText();
    public string AttackText();
    public string ShieldText();
    public string HealthText();
    public Sprite SpriteDisplay();
    public string CardText();
}
