using UnityEngine;

public class UserObject : MonoBehaviour {

    [SerializeField] private User _users;

    public User Users {
        get {
            return _users;
        }

        set {
            _users = value;
        }
    }



}
