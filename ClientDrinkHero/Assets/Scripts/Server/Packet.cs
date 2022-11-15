using System;
using UnityEngine;

[Serializable]
public class Packet {

    [SerializeField] private string _className;
    [SerializeField] private string _data;


    public Packet() {



    }

    public void SetData<T>(T data) {
        _className = typeof(T).ToString();

        if (_className == typeof(string).ToString()) {
            _data = data as string;
        }
        else {
            _data = JsonUtility.ToJson(data);
        }

    }

    public string ClassName {
        get {
            return _className;
        }


    }

    public string Data {
        get {
            return _data;
        }


    }
}
