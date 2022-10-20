using UnityEngine;

public class ServerObject : MonoBehaviour {


    private DrinkHeroServer _server;
    // Start is called before the first frame update
    void Start() {
        _server = new DrinkHeroServer();
        _server.StartServer();
    }



}
