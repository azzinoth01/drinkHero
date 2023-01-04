using System.Threading;
using UnityEngine;

public class NetworkConnection : MonoBehaviour {



    [SerializeField] public int port = 6969;
    [SerializeField] public string host = "markusdullnig.de";
    private Thread _thread;


    private void Awake() {
        StartServerConnection();
    }

    private void Update() {
        if (ServerRequests.checkUpdates.Count == 0) {
            return;
        }

        ServerRequests.checkUpdates.Dequeue();
        HandleRequests.Instance.CheckUpdateList();
        return;
    }

    private void OnEnable() {
        DontDestroyOnLoad(this);
    }

    private void OnDisable() {
        ReadServerDataThread.KeepRunning = false;

    }

    private void StartServerConnection() {

        ReadServerDataThread serverDataReadThread = new ReadServerDataThread(host, port, 3000);

        _thread = new Thread(serverDataReadThread.ThreadLoop);

        _thread.Start();

    }

}
