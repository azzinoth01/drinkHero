[Table("Item")]
public class UpgradeItemDatabase : DatabaseItem {
    private int _id;
    private string _name;
    private string _text;

    [Column("ID"), PrimaryKey]
    public int Id {
        get {
            return _id;
        }

        set {
            _id = value;
        }
    }
    [Column("Name")]
    public string Name {
        get {
            return _name;
        }

        set {
            _name = value;
        }
    }
    [Column("Text")]
    public string Text {
        get {
            return _text;
        }

        set {
            _text = value;
        }
    }

    public UpgradeItemDatabase() {

    }
}
