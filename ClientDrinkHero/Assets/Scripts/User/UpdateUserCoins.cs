using TMPro;
using UnityEngine;

public class UpdateUserCoins : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _counter;

    // Update is called once per frame
    void Update() {
        _counter.SetText(UserSingelton.Instance.UserObject.User.Money.ToString());
    }
}
