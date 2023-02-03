using UnityEngine;

public class UserObject : MonoBehaviour {

    private UserLoader _userHandle;
    [SerializeField] private UserDatabase _user;

    public UserDatabase User {
        get {
            return _user;
        }

        set {
            _user = value;
        }
    }

    void Start() {

        DontDestroyOnLoad(this);

        _userHandle = new UserLoader();

        _userHandle.LoadingFinished += UserLoaded;

        _userHandle.RequestData();

        UserSingelton.Instance.UserObject = this;
    }


    private void Update() {
        _userHandle.Update();
    }

    private void UserLoaded() {
        User = _userHandle.user;
    }
    private void OnDestroy() {
        _userHandle.LoadingFinished -= UserLoaded;
    }

    public void UpdateUserDataRequest(string request) {
        _userHandle.RequestData(request);
    }

}
