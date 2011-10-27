using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

using log4net;

using IronSmarkets.Clients;
using IronSmarkets.Sessions;
using IronSmarkets.Sockets;

namespace IronSmarkets.ConsoleExample
{
    public static class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(
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

            Log.Info("Application start");

            string host = args[0];
            int port = int.Parse(args[1]);
            string username = args[2];
            string password = args[3];
            ISocketSettings sockSettings = new SocketSettings(
                host, host, port, true, ValidateServerCertificate);
            ISessionSettings sessSettings = new SessionSettings(
                username, password);
            Log.Info("Logging in...");
            using (var client = SmarketsClient.Create(sockSettings, sessSettings))
            {
                Log.Info("Connected, logging out...");
                client.Logout();
                Log.Info("Logout returned, cleaning up...");
            }

            Console.WriteLine("Press <Enter> to exit...");
            Console.ReadLine();
        }
    }
}
