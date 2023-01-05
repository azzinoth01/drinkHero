public class UIDataContainer {


    private static UIDataContainer _instance;
    private IPlayer _player;
    private ICharacter _enemy;

    private IWaitingPanel _waitingPanel;

    public static UIDataContainer Instance {
        get {
            if (_instance == null) {
                _instance = new UIDataContainer();
            }
            return _instance;
        }
    }

    public IPlayer Player {
        get {
            return _player;
        }

        set {
            _player = value;
        }
    }

    public ICharacter Enemy {
        get {
            return _enemy;
        }

        set {
            _enemy = value;
        }
    }

    public IWaitingPanel WaitingPanel {
        get {
            return _waitingPanel;
        }
        set {
            _waitingPanel = value;
        }

    }
}
