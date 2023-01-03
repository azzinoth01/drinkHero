public class UserSingelton {


    private static UserSingelton _instance;
    private UserObject _userObject;


    public static UserSingelton Instance {
        get {
            if (_instance == null) {
                _instance = new UserSingelton();
            }
            return _instance;
        }

    }

    public UserObject UserObject {
        get {
            return _userObject;
        }

        set {
            _userObject = value;
        }
    }
}
