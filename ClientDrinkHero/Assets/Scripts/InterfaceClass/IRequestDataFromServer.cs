using System;

public interface IRequestDataFromServer {



    public int SendRequest(string functionCall, Type type);
    public void WriteBackData(string data, int id);
    public DataRequestStatusEnum RequestStatus(int id);

}
