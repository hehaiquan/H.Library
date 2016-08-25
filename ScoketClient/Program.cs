using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SocketClient
{
    class Program
    {
        private static byte[] result = new byte[1024];
        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //配置服务器IP与端口
                clientSocket.Connect(new IPEndPoint(ip, 8885));
                Console.WriteLine("连接服务器成功");
            }
            catch 
            {
                Console.WriteLine("连接服务器失败，请按回车键退出！");
                throw;
            }

            //通过clientSocket接收数据
            int receiveLength = clientSocket.Receive(result);
            Console.WriteLine("接收服务器消息：{0}", Encoding.ASCII.GetString(result, 0, receiveLength));

            for (int i = 0; i < 10; i++)
            {
                try
                {
                    Thread.Sleep(1000);
                    string sendMessage = "Client send Message Hello" + DateTime.Now;
                    clientSocket.Send(Encoding.ASCII.GetBytes(sendMessage));
                    Console.WriteLine("向服务器发送消息：{0}" + sendMessage);

                }
                catch
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    throw;
                }
            }

            Console.WriteLine("发送完毕，按回车键退出！");
            Console.ReadLine();
        }
    }
}
