using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string strPort = "211";
            string strIP = "127.0.0.1";
            if (args?.Length > 0)
            {
                if (args.Count(t => t.Contains("--port=")) > 0)
                {
                    var first = args.First(t => t.Contains("--port="));
                    strPort = first.Split("=")[1];
                }

                if (args.Count(t => t.Contains("--ip=")) > 0)
                {
                    var first = args.First(t => t.Contains("--ip="));
                    strIP = first.Split("=")[1];
                }
            }

            int port = int.Parse(strPort);

            IPAddress ip = IPAddress.Parse(strIP);
            IPEndPoint iPEndPoint = new IPEndPoint(ip, port);

            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(iPEndPoint);

            //send message
            string sendStr = "send to server : hello,ni hao";
            byte[] sendBytes = Encoding.ASCII.GetBytes(sendStr);
            sendBytes = sendBytes.Append((byte)23).Append((byte)23).ToArray();
            string ss = Encoding.ASCII.GetString(sendBytes);
            clientSocket.Send(sendBytes);

            //receive message
            string recStr = "";
            byte[] recBytes = new byte[4096];
            int bytes = clientSocket.Receive(recBytes, recBytes.Length, 0);
            recStr += Encoding.ASCII.GetString(recBytes, 0, bytes);
            Console.WriteLine(recStr);

            clientSocket.Close();
        }
    }
}
