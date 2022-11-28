using System;
using System.Collections.Generic;

public static class ServerRequests {
    public static Queue<string> serverRequestQueue;
    public static Queue<(int, Type)> writeServerDataToHandleRequests;
    public static Queue<bool> checkUpdates;

}
