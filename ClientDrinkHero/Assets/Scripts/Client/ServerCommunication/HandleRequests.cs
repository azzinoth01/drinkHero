using System;
using System.Collections.Generic;

public class HandleRequests : IHandleRequest {



    private static HandleRequests _instance;

    public static HandleRequests Instance {
        get {
            if (_instance == null) {
                _instance = new HandleRequests();
            }
            return _instance;
        }

        set {
            _instance = value;
        }
    }

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

    public int HandleRequest(string functionCall, Type type) {
        int index = LastIndex;

        ServerRequests.writeServerDataToHandleRequests.Enqueue((index, type));
        ServerRequests.serverRequestQueue.Enqueue(functionCall);
        RequestDataStatus[index] = DataRequestStatusEnum.Requested;
        RequestDataType[index] = type;

        LastIndex = index + 1;
        return index;

    }

    private HandleRequests() {
        LastIndex = 0;
        RequestData = new Dictionary<int, string>();
        RequestDataStatus = new Dictionary<int, DataRequestStatusEnum>();
        RequestDataType = new Dictionary<int, Type>();
    }
}
