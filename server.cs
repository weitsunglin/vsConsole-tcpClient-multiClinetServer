using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Server{
    class Program{
        static int PROTOCOL = 7000;
        private static TcpListener tcpListener;
        private static List<TcpClient> Clients = new List<TcpClient>();
        static void Main(string[] args){
            InitTcpListener();
        }
        public static void InitTcpListener(){
            tcpListener = new TcpListener(IPAddress.Any, PROTOCOL);
            tcpListener.Start();

            Console.WriteLine("tcpListener start");
            while (true){
                TcpClient client = tcpListener.AcceptTcpClient();
                if(client != null){
                    StreamReader reader = new StreamReader(client.GetStream());
                    string message = reader.ReadLine();
                    Console.WriteLine("new client is adding ID:"+message);
                    Clients.Add(client);
                    Thread thread = new Thread(ClientListener);
                    thread.Start(client);
                    continue;
                }
            }
        }
        public static void ClientListener(object clientObj)
        {
            TcpClient client = (TcpClient)clientObj;
            StreamReader reader = new StreamReader(client.GetStream());
            while (true){
                string message = reader.ReadLine();
                BroadCast(message, client);
                Console.WriteLine(message, "message from client");
            }
        }
        public static void BroadCast(string msg, TcpClient excludeClient){
            foreach (TcpClient client in Clients){
                if (client != excludeClient){
                    StreamWriter sWriter = new StreamWriter(client.GetStream());
                    sWriter.WriteLine(msg);
                    sWriter.Flush();
                    Console.WriteLine("BroadCast to client Success");
                }
            }
        }
    }
}
