using System;

public class MainClass {




    public static int Main() {

        bool _keepRunning = true;

        DrinkHeroServer server = new DrinkHeroServer();
        server.StartServer();
        Console.Write("start");
        AppDomain.CurrentDomain.ProcessExit += delegate {
            _keepRunning = false;
            if (server != null) {
                server.CloseServer();
            }

        };

        while (_keepRunning) {

            Thread.Sleep(1000);
        }
        if (server != null) {
            server.CloseServer();
        }
        Console.Write("closes");


        return 0;
    }


}