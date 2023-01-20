using System;

public class ConnectionScreenChecker : IWaitingPanel, IWaitForServer {


    private int _waitingOnServerAmount;
    public event Action<bool> DisplayWaitingPanel;
    private bool _waitingState;

    public bool WaitingState {
        get {
            return _waitingState;
        }
    }

    public void AddWaitOnServer() {
        _waitingOnServerAmount = _waitingOnServerAmount + 1;
        _waitingState = _waitingOnServerAmount > 0;
        DisplayWaitingPanel?.Invoke(_waitingState);

    }
    public void FinishedWaitOnServer() {
        _waitingOnServerAmount = _waitingOnServerAmount - 1;
        _waitingState = _waitingOnServerAmount > 0;
        DisplayWaitingPanel?.Invoke(_waitingState);
    }

    public ConnectionScreenChecker() {
        _waitingOnServerAmount = 0;

        NetworkDataContainer.Instance.WaitForServer = this;
        UIDataContainer.Instance.WaitingPanel = this;
    }
}
