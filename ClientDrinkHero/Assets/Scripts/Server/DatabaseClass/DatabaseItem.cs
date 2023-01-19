

using System.Runtime.CompilerServices;
#if CLIENT
using System;
using System.Collections.Generic;
using System.Reflection;

#endif


#if CLIENT
public abstract class DatabaseItem : IRequestDataFromServer, IGetUpdateFromServer {
#endif
#if SERVER
public abstract class DatabaseItem {
#endif
    protected Dictionary<int, string> _propertyToRequestedId;
    protected int _waitingOnDataCount;

    public int WaitingOnDataCount {
        get {
            return _waitingOnDataCount;
        }

    }
#if CLIENT

    public DatabaseItem() {
        _waitingOnDataCount = 0;
        _propertyToRequestedId = new Dictionary<int, string>();

    }
#endif

    protected static string GetPropertyName([CallerMemberName] string name = "") {

        return name;
    }

#if CLIENT
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
        HandleRequests.Instance.AddToUpdateList(this);
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
            List<HeroDatabase> list = HeroDatabase.CreateObjectDataFromString(data);

            if (isList == false) {
                info.SetValue(this, list[0]);
            }
            else {
                info.SetValue(this, list);
            }

        }
        else if (type == typeof(CardDatabase)) {
            List<CardDatabase> list = CardDatabase.CreateObjectDataFromString(data);
            if (isList == false) {
                info.SetValue(this, list[0]);
            }
            else {
                info.SetValue(this, list);
            }
        }
        else if (type == typeof(CardToHero)) {
            List<CardToHero> list = CardToHero.CreateObjectDataFromString(data);
            if (isList == false) {
                info.SetValue(this, list[0]);
            }
            else {
                info.SetValue(this, list);
            }
        }
        else if (type == typeof(EnemyDatabase)) {
            List<EnemyDatabase> list = EnemyDatabase.CreateObjectDataFromString(data);
            if (isList == false) {
                info.SetValue(this, list[0]);
            }
            else {
                info.SetValue(this, list);
            }
        }
        else if (type == typeof(EnemyToEnemySkill)) {
            List<EnemyToEnemySkill> list = EnemyToEnemySkill.CreateObjectDataFromString(data);
            if (isList == false) {
                info.SetValue(this, list[0]);
            }
            else {
                info.SetValue(this, list);
            }
        }
        else if (type == typeof(EnemySkillDatabase)) {
            List<EnemySkillDatabase> list = EnemySkillDatabase.CreateObjectDataFromString(data);
            if (isList == false) {
                info.SetValue(this, list[0]);
            }
            else {
                info.SetValue(this, list);
            }
        }
        else if (type == typeof(CardToEffect)) {
            List<CardToEffect> list = CardToEffect.CreateObjectDataFromString(data);
            if (isList == false) {
                info.SetValue(this, list[0]);
            }
            else {
                info.SetValue(this, list);
            }
        }
        else if (type == typeof(Effect)) {
            List<Effect> list = Effect.CreateObjectDataFromString(data);
            if (isList == false) {
                info.SetValue(this, list[0]);
            }
            else {
                info.SetValue(this, list);
            }
        }
        else if (type == typeof(UserDatabase)) {
            List<UserDatabase> list = UserDatabase.CreateObjectDataFromString(data);
            if (isList == false) {
                info.SetValue(this, list[0]);
            }
            else {
                info.SetValue(this, list);
            }
        }
        else if (type == typeof(HeroToUserDatabase)) {
            List<HeroToUserDatabase> list = HeroToUserDatabase.CreateObjectDataFromString(data);
            if (isList == false) {
                info.SetValue(this, list[0]);
            }
            else {
                info.SetValue(this, list);
            }
        }
        else if (type == typeof(ResponsMessageObject)) {
            List<ResponsMessageObject> list = ResponsMessageObject.CreateObjectDataFromString(data);
            if (isList == false) {
                info.SetValue(this, list[0]);
            }
            else {
                info.SetValue(this, list);
            }
        }
        else if (type == typeof(PullHistoryDatabase)) {
            List<PullHistoryDatabase> list = PullHistoryDatabase.CreateObjectDataFromString(data);
            if (isList == false) {
                info.SetValue(this, list[0]);
            }
            else {
                info.SetValue(this, list);
            }
        }
        else if (type == typeof(GachaDatabase)) {
            List<GachaDatabase> list = GachaDatabase.CreateObjectDataFromString(data);
            if (isList == false) {
                info.SetValue(this, list[0]);
            }
            else {
                info.SetValue(this, list);
            }
        }
        else if (type == typeof(GachaCategorieDatabase)) {
            List<GachaCategorieDatabase> list = GachaCategorieDatabase.CreateObjectDataFromString(data);
            if (isList == false) {
                info.SetValue(this, list[0]);
            }
            else {
                info.SetValue(this, list);
            }
        }
        else if (type == typeof(GachaToGachaCategorieDatabase)) {
            List<GachaToGachaCategorieDatabase> list = GachaToGachaCategorieDatabase.CreateObjectDataFromString(data);
            if (isList == false) {
                info.SetValue(this, list[0]);
            }
            else {
                info.SetValue(this, list);
            }
        }
        else if (type == typeof(GachaCategorieToGachaItemDatabase)) {
            List<GachaCategorieToGachaItemDatabase> list = GachaCategorieToGachaItemDatabase.CreateObjectDataFromString(data);
            if (isList == false) {
                info.SetValue(this, list[0]);
            }
            else {
                info.SetValue(this, list);
            }
        }
    }

    public bool GetUpdateFromServer() {
        CheckRequestedData();
        if (_waitingOnDataCount > 0) {
            return false;
        }

        return true;
    }



    public virtual void RequestLoadReferenzData() {

    }

#endif
}
