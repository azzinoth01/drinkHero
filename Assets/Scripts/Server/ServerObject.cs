using Mono.Data.Sqlite;
using System.IO;
using System.Text;
using UnityEngine;

public class ServerObject : MonoBehaviour {


    private DrinkHeroServer _server;
    // Start is called before the first frame update
    void Start() {
        _server = new DrinkHeroServer();
        _server.StartServer();
    }

    private void OnDisable() {
        _server.CloseServer();
    }

    [ContextMenu("test Server call")]
    public void TestServerFunction() {
        string testfilepath = Application.dataPath + "\\testfile.txt";
        FileStream file = new FileStream(testfilepath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

        StreamWriter writer = new StreamWriter(file, Encoding.UTF8, 1024);


        string connectionString = Application.dataPath;

        int pos = connectionString.LastIndexOf("/");

        connectionString = connectionString.Substring(0, pos);

        connectionString = connectionString + "/" + "DrinkHeroDatabase.db";

        connectionString = "URI=file:" + connectionString;

        SqliteConnection databaseConnection = new SqliteConnection(connectionString);

        databaseConnection.Open();
        DatabaseManager.Db = databaseConnection;

        //string test = TransmissionControl.EvaluateMessage(writer, "OBJECT COMMAND GetHeros END OBJECT COMMAND GetHeros END");

        //TransmissionControl.EvaluateMessage(writer, test);


        //TransmissionControl.EvaluateMessage(writer, "OBJECT VALUE classname\"Hero\";ID\"1\";Shield\"1\";Health\"2\";SpritePath\"SpritePath\";Name\"Name\" NEXT OBJECT VALUE classname\"Hero\";ID\"2\";Shield\"1\";Health\"2\";SpritePath\"SpritePath\";Name\"Name\" NEXT OBJECT VALUE classname\"Hero\";ID\"3\";Shield\"5\";Health\"10\";SpritePath\"\";Name\"heroName1\" NEXT OBJECT VALUE classname\"Hero\";ID\"4\";Shield\"3\";Health\"20\";SpritePath\"\";Name\"heroName4\" END ");

        writer.Close();
        file.Close();
    }


}
