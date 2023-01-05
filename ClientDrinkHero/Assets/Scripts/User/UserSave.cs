using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class UserSave {
    private int _id;

    public int Id {
        get {
            return _id;
        }

        set {
            _id = value;
        }
    }

    public UserSave() {
        _id = -1;
    }

    public void SaveUserID() {
        using (FileStream file = File.Create(Application.persistentDataPath + "/UserIdSave.sav")) {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, this);
        }
    }

    public static UserSave LoadSave() {

        UserSave s = new UserSave();


        if (System.IO.File.Exists(Application.persistentDataPath + "/UserIdSave.sav")) {

            BinaryFormatter bf = new BinaryFormatter();

            using (FileStream file = File.Open(Application.persistentDataPath + "/UserIdSave.sav", FileMode.Open)) {
                s = (UserSave)bf.Deserialize(file);
            }

            if (s == null) {
                return new UserSave();
            }
            return s;

        }
        return s;

    }
}
