using System;
using System.Collections.Generic;

public interface IHandleRequest {


    public int LastIndex {
        get;
        set;
    }
    public Dictionary<int, DataRequestStatusEnum> RequestDataStatus {
        get;
        set;
    }
    public Dictionary<int, string> RequestData {
        get;
        set;
    }
    public Dictionary<int, Type> RequestDataType {
        get;
        set;
    }

    public int HandleRequest(string functionCall, Type type);

}
