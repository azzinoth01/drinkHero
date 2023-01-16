public class GachaPullRange {


    private int _from;
    private int _to;
    private List<GachaPullRange> _gachaPullRanges;
    private int _gachaPullRangeMaxValue;
    private int _id;



    public GachaPullRange() {
        _gachaPullRanges = new List<GachaPullRange>();
        _id = -1;

        _from = 0;
        _to = 0;
        _gachaPullRangeMaxValue = 0;
    }

    public int From {
        get {
            return _from;
        }

        set {
            _from = value;
        }
    }

    public int To {
        get {
            return _to;
        }

        set {
            _to = value;
        }
    }

    public int Id {
        get {
            return _id;
        }

        set {
            _id = value;
        }
    }

    public List<GachaPullRange> GachaPullRanges {
        get {
            return _gachaPullRanges;
        }
    }

    public int GachaPullRangeMaxValue {
        get {
            return _gachaPullRangeMaxValue;
        }

        set {
            _gachaPullRangeMaxValue = value;
        }
    }


}

