[System.AttributeUsage(System.AttributeTargets.Property)]
public class ColumnAttribute : System.Attribute {

    private string _name;


    public ColumnAttribute(string name) {
        _name = name;


    }

    public string Name {
        get {
            return _name;
        }


    }
}
