

public class MainClass {




    public static int Main() {

        LogManager logger = new LogManager(300);


        DrinkHeroServer server = new DrinkHeroServer();
        server.StartServer();


        AppDomain.CurrentDomain.ProcessExit += delegate {
            logger.KeepRunning = false;
            if (server != null) {
                server.CloseServer();

            }
            if (logger != null) {
                logger.WriteLog();
            }


        };
        logger.ManageWrite();



        return 0;
    }


}