using UnityEngine;

public class UserObject : MonoBehaviour {

    private UserLoader _userHandle;
    public UserDatabase _user;

    void Start() {

        DontDestroyOnLoad(this);

        _userHandle = new UserLoader();

        _userHandle.LoadingFinished += UserLoaded;

        UserSingelton.Instance.UserObject = this;
    }


    private void Update() {
        _userHandle.Update();
    }

    private void UserLoaded() {
        _user = _userHandle.user;
    }
    private void OnDestroy() {
        _userHandle.LoadingFinished -= UserLoaded;
    }

}
