using System;
using System.Net.Sockets;
using System.IO;
using System.Threading;
namespace Client{
    class Program{
        static int PROTOCOL = 7000;
        static void Read(object obj){
            TcpClient tcpClient = (TcpClient)obj; //轉型
            StreamReader sReader = new StreamReader(tcpClient.GetStream());
            while (true){
                try{
                    string message = sReader.ReadLine();
                    Console.WriteLine(message);
                }
                catch (Exception e){
                    Console.WriteLine(e.Message);
                    break;
                }
            }
        }
        
        static void Main(string[] args){

            Random rnd = new Random();
            int ID = rnd.Next(0, 999999);
            TcpClient tcpClient = new TcpClient("127.0.0.1", PROTOCOL);
            StreamWriter sWriter = new StreamWriter(tcpClient.GetStream());
            if (tcpClient.Connected){
                Console.WriteLine("tcpClient"+ ID.ToString()+ "Connected to server");
                sWriter.WriteLine(ID.ToString());
                sWriter.Flush();
            }
            Thread thread = new Thread(Read);
            thread.Start(tcpClient);           
            try{     
                while (true){
                    if (tcpClient.Connected){
                        string input = Console.ReadLine();
                        sWriter.WriteLine("clientID"+ID.ToString()+":"+ input);
                        sWriter.Flush();
                    }
                }
            }
            catch (Exception e){
                Console.Write(e.Message);
            }
            Console.ReadKey();
            Console.WriteLine("Client close");
        }
    }
}