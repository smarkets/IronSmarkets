using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

using IronSmarkets.Sessions;
using IronSmarkets.Sockets;

namespace IronSmarkets.ConsoleExample
{
    class Program
    {
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
                Console.WriteLine("Usage: IronSmarkets.ConsoleExample.exe <host> <port> <username> <password>");
                return;
            }

            string host = args[0];
            int port = int.Parse(args[1]);
            string username = args[2];
            string password = args[3];
            ISocketSettings sockSettings = new SocketSettings(
                host, host, port, true, ValidateServerCertificate);
            ISessionSettings sessSettings = new SessionSettings(
                username, password);
            using (var session = new SeqSession(sockSettings, sessSettings))
            {
                Console.WriteLine("Connecting...");
                session.Login();
                Console.WriteLine("Connected, logging out...");
                session.Logout();
            }
        }
    }
}
