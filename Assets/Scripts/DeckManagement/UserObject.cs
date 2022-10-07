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





    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
