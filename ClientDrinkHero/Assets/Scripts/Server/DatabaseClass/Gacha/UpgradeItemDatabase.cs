using System.Collections.Generic;
using System.Reflection;

[Table("Item")]
public class UpgradeItemDatabase : DatabaseItem {
    private int _id;
    private string _name;
    private string _text;
    private string _spritePath;

    private static Dictionary<string, UpgradeItemDatabase> _cachedData = new Dictionary<string, UpgradeItemDatabase>();


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
    [Column("SpritePath")]
    public string SpritePath {
        get {
            return _spritePath;
        }

        set {
            _spritePath = value;
        }
    }
#if CLIENT

    public static List<UpgradeItemDatabase> CreateObjectDataFromString(string message) {

        List<UpgradeItemDatabase> list = new List<UpgradeItemDatabase>();

        List<string[]> objectStrings = DatabaseItemCreationHelper.GetObjectStrings(message);
        TableMapping mapping = DatabaseManager.GetTableMapping<UpgradeItemDatabase>();

        foreach (string[] obj in objectStrings) {
            UpgradeItemDatabase item = new UpgradeItemDatabase();
            foreach (string parameter in obj) {
                string parameterName = RegexPatterns.PropertyName.Match(parameter).Value;
                string parameterValue = RegexPatterns.PropertyValue.Match(parameter).Value;

                if (parameterName == mapping.PrimaryKeyColumn) {
                    if (_cachedData.TryGetValue(parameterValue, out UpgradeItemDatabase existingItem)) {
                        item = existingItem;
                        break;
                    }
                    else {
                        _cachedData.Add(parameterValue, item);
                    }

                }

                if (mapping.ColumnsMapping.TryGetValue(parameterName, out string property)) {
                    PropertyInfo info = item.GetType().GetProperty(property);
                    DatabaseItemCreationHelper.ParseParameterValues(item, info, parameterValue);
                }
            }
            list.Add(item);
        }

        return list;
    }
#endif
    public UpgradeItemDatabase() {

    }
}
