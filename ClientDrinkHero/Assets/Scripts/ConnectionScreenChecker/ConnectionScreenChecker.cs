using System;

public class ConnectionScreenChecker : IWaitingPanel, IWaitForServer {


    private int _waitingOnServerAmount;
    public event Action<bool> DisplayWaitingPanel;



    public void AddWaitOnServer() {
        _waitingOnServerAmount = _waitingOnServerAmount + 1;
        DisplayWaitingPanel?.Invoke(_waitingOnServerAmount > 0);
    }
    public void FinishedWaitOnServer() {
        _waitingOnServerAmount = _waitingOnServerAmount - 1;
        DisplayWaitingPanel?.Invoke(_waitingOnServerAmount > 0);
    }

    public ConnectionScreenChecker() {
        _waitingOnServerAmount = 0;

        NetworkDataContainer.Instance.WaitForServer = this;
        UIDataContainer.Instance.WaitingPanel = this;
    }
}
