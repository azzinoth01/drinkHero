public class UIDataContainer {

    private static UIDataContainer _instance;
    private IPlayer _player;
    private ICharacter _enemy;

    private IWaitingPanel _waitingPanel;

    private Dictionary<int, IAssetLoader> _characterSlots;
    private IAssetLoader _enemySlot;

    private IFlyingTextHandler _enemyText;
    private IFlyingTextHandler _playerText;


    public static int[] TeamIds;

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

    public Dictionary<int, IAssetLoader> CharacterSlots {
        get {
            return _characterSlots;
        }

    }

    public IAssetLoader EnemySlot {
        get {
            return _enemySlot;
        }

        set {
            _enemySlot = value;
        }
    }

    public IFlyingTextHandler EnemyText {
        get {
            return _enemyText;
        }

        set {
            _enemyText = value;
        }
    }

    public IFlyingTextHandler PlayerText {
        get {
            return _playerText;
        }

        set {
            _playerText = value;
        }
    }

    private UIDataContainer() {
        _characterSlots = new Dictionary<int, IAssetLoader>();
    }
}
