using System;
using System.Collections.Generic;

public class HandleRequests : IHandleRequest {



    private static HandleRequests _instance;
    private List<IGetUpdateFromServer> _updateList;

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

    public Dictionary<int, string> ServerRequestStrings {
        get;
        set;
    }

    public int HandleRequest(string functionCall, Type type) {
        int index = LastIndex;


        ServerRequestStrings.Add(index, functionCall);

        ServerRequests.writeServerDataToHandleRequests.Enqueue((index, type));
        ServerRequests.serverRequestQueue.Enqueue(functionCall);
        RequestDataStatus[index] = DataRequestStatusEnum.Requested;
        RequestDataType[index] = type;

        LastIndex = index + 1;
        return index;

    }

    public void AddToUpdateList(IGetUpdateFromServer obj) {
        if (_updateList.Contains(obj) == false) {
            _updateList.Add(obj);
        }

    }

    public void CheckUpdateList() {
        for (int i = _updateList.Count; i > 0;) {
            i = i - 1;

            if (_updateList[i].GetUpdateFromServer()) {
                _updateList.RemoveAt(i);
            }
        }
    }

    private HandleRequests() {
        LastIndex = 0;
        ServerRequestStrings = new Dictionary<int, string>();
        _updateList = new List<IGetUpdateFromServer>();
        RequestData = new Dictionary<int, string>();
        RequestDataStatus = new Dictionary<int, DataRequestStatusEnum>();
        RequestDataType = new Dictionary<int, Type>();
    }
}
