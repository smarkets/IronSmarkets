// Copyright (c) 2011 Smarkets Limited
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy,
// modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
// BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN
// ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Linq;
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
                client.Login();
                Log.Info("Connected");
                foreach (var ping in Enumerable.Range(1, 5))
                {
                    Log.Debug(
                        string.Format(
                            "[{0}] Sent ping with sequence {1}",
                            ping, client.Ping()));
                }
                foreach (var payload in client.Logout())
                {
                    Log.Debug(
                        string.Format(
                            "Got a payload {0} / {1} when logging out",
                            payload.Type, payload.EtoPayload.Type));
                }
                Log.Info("Logout returned, cleaning up...");
            }

            Console.WriteLine("Press <Enter> to exit...");
            Console.ReadLine();
        }
    }
}
