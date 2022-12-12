using System;

public interface IWaitingPanel
{
    public event Action<bool> DisplayWaitingPanel;
}