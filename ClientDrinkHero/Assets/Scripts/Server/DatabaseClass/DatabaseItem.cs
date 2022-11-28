using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

public abstract class DatabaseItem : IRequestDataFromServer {

    // Database object baseclass to limit database operations to child classes

    protected Dictionary<int, string> _propertyToRequestedId;
    protected int _waitingOnDataCount;

    public int WaitingOnDataCount {
        get {
            return _waitingOnDataCount;
        }

        set {
            _waitingOnDataCount = value;
        }
    }

    public DatabaseItem() {
        _waitingOnDataCount = 0;
        _propertyToRequestedId = new Dictionary<int, string>();
    }


    protected static string GetPropertyName([CallerMemberName] string name = "") {

        return name;
    }


    public virtual bool AlreadyRequested(string property) {

        foreach (KeyValuePair<int, string> pair in _propertyToRequestedId) {
            if (property == pair.Value) {
                if (RequestStatus(pair.Key) == DataRequestStatusEnum.None) {
                    return false;
                }
                else {
                    return true;
                }
            }
        }

        return false;
    }

    public virtual DataRequestStatusEnum RequestStatus(int id) {
        if (HandleRequests.Instance.RequestDataStatus.TryGetValue(id, out DataRequestStatusEnum status)) {
            return status;
        }
        else {
            return DataRequestStatusEnum.None;
        }
    }
    public virtual int SendRequest(string functionCall, Type type) {
        _waitingOnDataCount = _waitingOnDataCount + 1;
        return HandleRequests.Instance.HandleRequest(functionCall, type);
    }
    public virtual void WriteBackData(string data, int id) {
        if (_propertyToRequestedId.TryGetValue(id, out string name)) {
            PropertyInfo info = GetType().GetProperty(name);
            Type type = HandleRequests.Instance.RequestDataType[id];
            SetDataToProperty(data, type, info);


            HandleRequests.Instance.RequestDataStatus[id] = DataRequestStatusEnum.RecievedAccepted;
            _waitingOnDataCount = _waitingOnDataCount - 1;
        }
    }
    public virtual void CheckRequestedData() {

        foreach (KeyValuePair<int, string> pair in _propertyToRequestedId) {
            if (HandleRequests.Instance.RequestDataStatus[pair.Key] == DataRequestStatusEnum.Recieved) {
                WriteBackData(HandleRequests.Instance.RequestData[pair.Key], pair.Key);
            }
        }
    }

    private void SetDataToProperty(string data, Type type, PropertyInfo info) {
        bool isList = false;
        if (info.PropertyType.IsGenericType && info.PropertyType.GetGenericTypeDefinition() == typeof(List<>)) {
            isList = true;
        }

        if (type == typeof(HeroDatabase)) {
            List<HeroDatabase> list = TransmissionControl.GetObjectData<HeroDatabase>(data);

            if (isList == false) {
                info.SetValue(this, list[0]);
            }
            else {
                info.SetValue(this, list);
            }

        }
        else if (type == typeof(CardDatabase)) {
            List<CardDatabase> list = TransmissionControl.GetObjectData<CardDatabase>(data);
            if (isList == false) {
                info.SetValue(this, list[0]);
            }
            else {
                info.SetValue(this, list);
            }
        }
        else if (type == typeof(CardToHero)) {
            List<CardToHero> list = TransmissionControl.GetObjectData<CardToHero>(data);
            if (isList == false) {
                info.SetValue(this, list[0]);
            }
            else {
                info.SetValue(this, list);
            }
        }
        else if (type == typeof(EnemyDatabase)) {
            List<EnemyDatabase> list = TransmissionControl.GetObjectData<EnemyDatabase>(data);
            if (isList == false) {
                info.SetValue(this, list[0]);
            }
            else {
                info.SetValue(this, list);
            }
        }
        else if (type == typeof(EnemyToEnemySkill)) {
            List<EnemyToEnemySkill> list = TransmissionControl.GetObjectData<EnemyToEnemySkill>(data);
            if (isList == false) {
                info.SetValue(this, list[0]);
            }
            else {
                info.SetValue(this, list);
            }
        }
        else if (type == typeof(EnemySkillDatabase)) {
            List<EnemySkillDatabase> list = TransmissionControl.GetObjectData<EnemySkillDatabase>(data);
            if (isList == false) {
                info.SetValue(this, list[0]);
            }
            else {
                info.SetValue(this, list);
            }
        }




    }

}
