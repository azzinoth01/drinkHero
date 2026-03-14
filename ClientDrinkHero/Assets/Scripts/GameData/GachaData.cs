using System;
using UnityEngine;

[Serializable]
public class GachaData
{
    [SerializeField] private string _id;
    [SerializeField] private string _name;
    [SerializeField] private string _costType;
    [SerializeField] private int _costSingelPull;
    [SerializeField] private int _costMultiPull;
    [SerializeField] private int _multiPullAmount;


    public GachaData() {
        _id = Guid.NewGuid().ToString("N");
    }

    public string Id {
        get {
            return _id;
        }
    }

    public string Name {
        get {
            return _name;
        }
    }

    public string CostType {
        get {
            return _costType;
        }
    }

    public int CostSingelPull {
        get {
            return _costSingelPull;
        }
    }

    public int CostMultiPull {
        get {
            return _costMultiPull;
        }
    }

    public int MultiPullAmount {
        get {
            return _multiPullAmount;
        }
    }
}