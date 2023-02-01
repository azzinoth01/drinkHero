using System;

public interface IWaitingPanel {
    public event Action<bool> DisplayWaitingPanel;
    public bool WaitingState {
        get;
    }
}