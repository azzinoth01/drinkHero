

public class LogManager {


    public static Queue<string> LogQueue;
    private bool _keepRunning;
    private int _logCheckTimeInSeconds;

    public LogManager(int checkTimeInSeconds) {
        _keepRunning = true;
        _logCheckTimeInSeconds = checkTimeInSeconds;
        LogQueue = new Queue<string>();
    }

    public bool KeepRunning {
        get {
            return _keepRunning;
        }

        set {
            _keepRunning = value;
        }
    }

    public void ManageWrite() {

        int currentSeconds = 0;
        while (_keepRunning) {
            if (LogQueue.Count >= 1000) {
                WriteLog();
            }
            else if (currentSeconds >= _logCheckTimeInSeconds) {
                currentSeconds = 0;
                if (LogQueue.Count != 0) {
                    WriteLog();
                }

            }
            Thread.Sleep(1000);
            currentSeconds = currentSeconds + 1;
        }

    }

    public void WriteLog() {


        try {
            DateOnly date = DateOnly.FromDateTime(DateTime.Now);
            string path = AppDomain.CurrentDomain.BaseDirectory + "Logs/" + date.ToString("dd_MM_yyyy") + "_ServerLog.txt";
            //Console.Write("Write logs to " + path + "\r\n");
            using (FileStream fileStream = new FileStream(path, FileMode.Append, FileAccess.Write)) {
                //Console.Write("opend file" + "\r\n");
                try {
                    using (StreamWriter writer = new StreamWriter(fileStream)) {
                        //Console.Write("start writing" + "\r\n");
                        while (LogQueue.Count != 0) {
                            try {
                                //Console.Write("write to file" + "\r\n");
                                writer.Write(LogQueue.Dequeue());
                            }
                            catch (Exception e) {
                                //Console.Write(e.Message + "\r\n");
                                break;
                            }
                        }
                    }
                }
                catch (Exception e) {
                    //Console.Write(e.Message + "\r\n");
                }


            }
        }
        catch (Exception e) {
            //Console.Write(e.Message + "\r\n");
        }

    }

}

