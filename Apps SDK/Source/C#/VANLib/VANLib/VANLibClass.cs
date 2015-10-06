using System;
using System.Text;
using System.Threading;
using ZeroMQ;

namespace VANLibClass
{
    public class VANLib
    {

        public delegate void InDelegate(string token, string message);
        public string Export(string token, InDelegate callback)
        {
            using (var context = ZmqContext.Create())
            using (var socket = context.CreateSocket(SocketType.SUB))
            {
                socket.Connect("tcp://localhost:22220");
                if (token == "")
                {
                    socket.SubscribeAll();
                }
                else
                {
                    byte[] UTF8bytes = Encoding.UTF8.GetBytes(token);
                    socket.Subscribe(UTF8bytes);
                }

                while (true)
                {
                    Frame request = null;
                    String envelope = null;
                    String message = null;
                    if (token == "")
                    {
                        request = socket.ReceiveFrame();
                        message = Encoding.UTF8.GetString(request);
                        callback(null, message);
                    }
                    else
                    {
                        request = socket.ReceiveFrame();
                        envelope = Encoding.UTF8.GetString(request);
                        request = socket.ReceiveFrame();
                        message = Encoding.UTF8.GetString(request);
                        callback(envelope, message);
                    }
                    Thread.Sleep(10);
                }
            }
        }

        public string Import(string token, string msg)
        {
            using (var context = ZmqContext.Create())
            using (var socket = context.CreateSocket(SocketType.REQ))
            {
                socket.Connect("tcp://localhost:22222");
                Thread.Sleep(10);
                socket.SendFrame(new Frame(Encoding.UTF8.GetBytes(token + "," + msg)));
                Frame request = socket.ReceiveFrame();
                String message = Encoding.UTF8.GetString(request);
                socket.Disconnect("tcp://localhost:22222");
                return message;
            }
        }

        public delegate string InOutDelegate(string token, string msg);
        public string Delegate(string token, InOutDelegate callback)
        {
            using (var context = ZmqContext.Create())
            using (var socket = context.CreateSocket(SocketType.SUB))
            {
                socket.Connect("tcp://localhost:22220");
                if (token == "")
                {
                    socket.SubscribeAll();
                }
                else
                {
                    byte[] UTF8bytes = Encoding.UTF8.GetBytes(token);
                    socket.Subscribe(UTF8bytes);
                }

                while (true)
                {
                    Frame request = null;
                    String envelope = null;
                    String message = null;
                    if (token == "")
                    {
                        request = socket.ReceiveFrame();
                        message = Encoding.UTF8.GetString(request);
                        Import(token, callback(null, message));
                    }
                    else
                    {
                        request = socket.ReceiveFrame();
                        envelope = Encoding.UTF8.GetString(request);
                        request = socket.ReceiveFrame();
                        message = Encoding.UTF8.GetString(request);
                        Import(token, callback(envelope, message));
                    }
                    Thread.Sleep(10);
                }
            }
        }

    }
}
