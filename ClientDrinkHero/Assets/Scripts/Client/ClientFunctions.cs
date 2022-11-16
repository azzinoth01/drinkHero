using System.Collections.Generic;
using System.Reflection;

public static class ClientFunctions {

    public static List<T> DataReadSequenze<T>() where T : new() {
        string readData = "";
        while (true) {
            char[] buffer = new char[1024];
            int readCount = GlobalGameInfos.Instance.Reader.Read(buffer, 0, 1024);

            if (readCount > 0) {
                readData = readData + new string(buffer, 0, readCount);
            }

            string message = TransmissionControl.GetMessageObject(readData, out readData);
            if (message == "" || message == null) {
                if (TransmissionControl.CheckIfDataIsEmpty(readData, out readData)) {
                    break;
                }
            }
            else {
                bool? isCommand = TransmissionControl.IsCommandMessage(message);

                if (isCommand == null) {
                    return null;
                }
                else if (isCommand == false) {

                    return TransmissionControl.GetObjectData<T>(message);
                }
                else {
                    return null;
                }

            }

        }
        return null;
    }




    public static string CreateFunctionCallString(string functionName, string parameter = null) {
        string functionCall = "OBJECT COMMAND " + functionName;
        if (parameter != null) {
            functionCall = functionCall + " PARAMETER " + parameter;
        }
        functionCall = functionCall + " END";

        return functionCall;

    }

    public static List<HeroDatabase> GetHeroDatabases() {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetHeros));

        string callName = info.GetCustomAttribute<ServerFunctionAttribute>().Name;

        string function = CreateFunctionCallString(callName);

        GlobalGameInfos.Instance.Writer.Write(function);



        return DataReadSequenze<HeroDatabase>();

    }
    //public static List<HeroDatabase> GetHeroDatabasesByKeyPair(string key, long id) {

    //}


}
