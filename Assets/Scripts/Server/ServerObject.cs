using UnityEngine;

public class ServerObject : MonoBehaviour {


    private DrinkHeroServer _server;
    // Start is called before the first frame update
    void Start() {
        _server = new DrinkHeroServer();
        _server.StartServer();
    }

    // Update is called once per frame
    void Update() {

    }


}
