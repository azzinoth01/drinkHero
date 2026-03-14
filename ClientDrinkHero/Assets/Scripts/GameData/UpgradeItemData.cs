using System;
using UnityEngine;

[Serializable]
public class UpgradeItemData
{
    [SerializeField] private string _id;
    [SerializeField] private string _name;
    [SerializeField] private string _text;
    [SerializeField] private string _spritePath;

    public UpgradeItemData() {
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

    public string Text {
        get {
            return _text;
        }
    }

    public string SpritePath {
        get {
            return _spritePath;
        }
    }
}