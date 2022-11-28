using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWaitingOnServer {

    public bool IsWaitingOnServer {
        get; set;
    }
    public bool GetUpdateFromServer();
}
