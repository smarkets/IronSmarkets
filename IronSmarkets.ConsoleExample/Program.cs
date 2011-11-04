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
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

using log4net;

using IronSmarkets.Clients;
using IronSmarkets.Data;
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

        static void MultiPing(ISmarketsClient client, int pingers, int syncPings)
        {
            var threads = new List<Thread>(pingers);
            foreach (var sleeper in Enumerable.Range(1, pingers))
            {
                int sleeper1 = sleeper;
                var t1 = new Thread(
                    () => {
                        Thread.Sleep(sleeper1 * 50);
                        var pingSeq = client.Ping();
                        Log.Debug(string.Format("Sent ping {0}", pingSeq));
                    }) {
                    Name = string.Format("pinger{0}", sleeper)
                };
                t1.Start();
                threads.Add(t1);
            }
            foreach (var ping in Enumerable.Range(1, syncPings))
            {
                Log.Debug(
                    string.Format(
                        "[{0}] Sent ping with sequence {1}",
                        ping, client.Ping()));
            }
            foreach (var thread in threads)
            {
                Log.Debug(string.Format("Joining {0}", thread.Name));
                thread.Join();
            }
        }

        static IEventMap GetEvents(ISmarketsClient client)
        {
            var builder = new EventQueryBuilder();
            builder.SetCategory("sport");
            builder.SetSport("football");
            builder.SetDateTime(DateTime.Today);
            var events = client.GetEvents(builder.GetResult()).Data;
            Log.Debug(string.Format("Got {0} events:", events.Count));
            foreach (var eventInfo in events)
            {
                Log.Debug(
                    string.Format(
                        "\t{0} => {1} ({2})",
                        eventInfo.Key,
                        eventInfo.Value.Name,
                        eventInfo.Value.Category));
            }
            return events;
        }

        static void Main(string[] args)
        {
            Thread.CurrentThread.Name = "main";
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
            IClientSettings settings = new ClientSettings(sockSettings, sessSettings);
            using (var client = SmarketsClient.Create(settings))
            {
                client.PayloadReceived += (sender, eargs) => Log.Info(
                    string.Format(
                        "Event fired for payload [{0}] {1} / {2}",
                        eargs.Sequence,
                        eargs.Payload.EtoPayload.Type,
                        eargs.Payload.Type));
                client.AddPayloadHandler(payload => true);
                client.Login();
                Log.Info("Connected");
                var acct = client.GetAccountState().Data;
                Log.Info(string.Format("Got account {0}", acct));
                Log.Debug("Calling client.Logout()");
                var logoutSeq = client.Logout();
                Log.Debug(string.Format("Logout seq was {0}", logoutSeq));
                Log.Info("Logout returned, cleaning up...");
            }

            Console.WriteLine("Press <Enter> to exit...");
            Console.ReadLine();
        }
    }
}
