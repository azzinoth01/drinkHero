[System.AttributeUsage(System.AttributeTargets.Method)]
public class ServerFunctionAttribute : System.Attribute {
    private string _name;

    public ServerFunctionAttribute(string name) {
        _name = name;
    }

    public string Name {
        get {
            return _name;
        }

    }
}
