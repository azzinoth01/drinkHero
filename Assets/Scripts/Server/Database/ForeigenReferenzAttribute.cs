[System.AttributeUsage(System.AttributeTargets.Property)]
public class ForeigenReferenzAttribute : System.Attribute {
    private string _property;

    public ForeigenReferenzAttribute(string foreigenKey) {
        _property = foreigenKey;
    }

    public string Property {
        get {
            return _property;
        }
    }
}
