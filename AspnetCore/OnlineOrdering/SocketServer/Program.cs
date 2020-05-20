using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace SocketServer
{
    public class SocketData
    {
        public Socket Sender { get; set; }

        public byte[] Datas { get; set; }

        public List<byte> RecordTotalBytes { get; set; }

        //public Guid 
    }

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

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(iPEndPoint);
            socket.Listen(0);
            Console.WriteLine("监听已经打开，请等待");

            //Socket serverSocket = socket.Accept();
            socket.BeginAccept(SocketAccept, socket);
            //var asyResult = socket.BeginAccept(null, null);
            //serverSocket.EndAccept()
            //Console.WriteLine("连接已经建立");
            //string recStr = "";
            //byte[] recByte = new byte[4096];
            //int bytes = serverSocket.Receive(recByte, recByte.Length, 0);
            //recStr += Encoding.ASCII.GetString(recByte, 0, bytes);

            ////send message
            //Console.WriteLine("服务器端获得信息:{0}", recStr);
            //string sendStr = "send to client :hello";
            //byte[] sendByte = Encoding.ASCII.GetBytes(sendStr);
            //serverSocket.Send(sendByte, sendByte.Length, 0);
            //serverSocket.Close();
            //socket.Close();
            Console.ReadKey();
        }

        private static void SocketAccept(IAsyncResult async)
        {
            Console.WriteLine("连接已经建立");
            Socket acceptSocket = (Socket)async.AsyncState;
            Socket newSocket = acceptSocket.EndAccept(async);
            // //等待新的客户端连接
            acceptSocket.BeginAccept(SocketAccept, acceptSocket);

            byte[] recByte = new byte[10];
            SocketData socketData = new SocketData() { Sender = newSocket, Datas = recByte, RecordTotalBytes = new List<byte>() };
            newSocket.BeginReceive(recByte, 0, recByte.Length, 0, SocketReceive, socketData);

        }

        private static void SocketReceive(IAsyncResult async)
        {
            SocketData socketData = (SocketData)async.AsyncState;
            Socket receiveSocket = socketData.Sender;

            int len = 0;
            try
            {
                len = receiveSocket.EndReceive(async);
            }
            catch (SocketException ex)
            {
                receiveSocket.Close();
                Console.WriteLine(ex.Message);
                return;
            }
            //读取的长度与buffer缓存的大小一致
            if (len == socketData.Datas?.Length)
            {
                socketData.RecordTotalBytes.AddRange(socketData.Datas);
            }
            else if (len > 0 && len < socketData.Datas?.Length)
            {
                socketData.RecordTotalBytes.AddRange(socketData.Datas.Take(len));
            }

            if (socketData.RecordTotalBytes?.Count > 2)
            {
                int receiveCount = socketData.RecordTotalBytes.Count;
                if (socketData.RecordTotalBytes[receiveCount - 1] == 23 && socketData.RecordTotalBytes[receiveCount - 2] == 23)
                {
                    Console.WriteLine(Encoding.ASCII.GetString(socketData.RecordTotalBytes.Take(receiveCount - 2).ToArray()));
                }
            }

            try
            {
                receiveSocket.BeginReceive(socketData.Datas, 0, socketData.Datas.Length, 0, SocketReceive, socketData);
            }
            catch (SocketException ex)
            {
                receiveSocket.Close();
                Console.WriteLine(ex.Message);
                return;
            }

            //receiveSocket.BeginReceive()
            Console.WriteLine(len);
        }


    }
}
