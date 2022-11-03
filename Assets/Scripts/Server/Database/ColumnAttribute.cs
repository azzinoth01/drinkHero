using System.Runtime.CompilerServices;

[System.AttributeUsage(System.AttributeTargets.Property)]
public class ColumnAttribute : System.Attribute {

    private string _name;
    public string caller;

    public ColumnAttribute(string name, [CallerMemberName] string nember = null) {
        _name = name;

        caller = nember;
    }

    public string Name {
        get {
            return _name;
        }


    }
}
