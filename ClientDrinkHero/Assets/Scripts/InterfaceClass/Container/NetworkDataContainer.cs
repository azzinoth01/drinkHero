public class NetworkDataContainer {
    private static NetworkDataContainer _instance;
    private IWaitForServer _waitForServer;


    public static NetworkDataContainer Instance {
        get {
            if (_instance == null) {
                _instance = new NetworkDataContainer();
            }
            return _instance;
        }
    }

    public IWaitForServer WaitForServer {
        get {
            return _waitForServer;
        }

        set {
            _waitForServer = value;
        }
    }


}
