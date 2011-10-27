using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

using log4net;

using IronSmarkets.Sessions;
using IronSmarkets.Sockets;

namespace IronSmarkets.ConsoleExample
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static bool ValidateServerCertificate(
            object sender,
            X509Certificate certficiate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            // WARNING: This does NO certificate validation and should
            // not be used in a production system!
            return true;
        }

        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine(
                    "Usage: IronSmarkets.ConsoleExample.exe " +
                    "<host> <port> <username> <password>");
                return;
            }

            log.Info("Application start");

            string host = args[0];
            int port = int.Parse(args[1]);
            string username = args[2];
            string password = args[3];
            ISocketSettings sockSettings = new SocketSettings(
                host, host, port, true, ValidateServerCertificate);
            ISessionSettings sessSettings = new SessionSettings(
                username, password);
            log.Info("Creating SeqSession");
            using (var session = new SeqSession(sockSettings, sessSettings))
            {
                log.Info("Logging in...");
                session.Login();
                log.Info("Connected, logging out...");
                session.Logout();
                log.Info("Logout returned, cleaning up...");
            }

            Console.WriteLine("Press <Enter> to exit...");
            Console.ReadLine();
        }
    }
}
