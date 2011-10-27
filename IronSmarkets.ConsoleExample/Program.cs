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
            ISocketSettings sockSettings = new SocketSettings(
                "api-sandbox.smarkets.com", "api-sandbox.smarkets.com",
                3701, true, new RemoteCertificateValidationCallback(
                    ValidateServerCertificate));
            ISessionSettings sessSettings = new SessionSettings(
                "username@domain.invalid", "password");
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
