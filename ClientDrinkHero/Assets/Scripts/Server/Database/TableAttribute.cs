[System.AttributeUsage(System.AttributeTargets.Class)]
public class TableAttribute : System.Attribute {
    private string _table;


    public TableAttribute(string name) {
        _table = name;




    }

    public string Table {
        get {
            return _table;
        }

    }



}
