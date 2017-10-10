﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkFramework
{
    public sealed class UDPChannel: Channel<UDPPacket>
    {
        private int _port;
        private UdpClient _server;

        public UDPChannel() {
            Console.Write("Listen on port: ");
            int port = int.Parse(Console.ReadLine());
            _port = port;
            _server = new UdpClient(port);
        }

        public void Send(UDPPacket pack)
        {
            byte[] data = Encoding.ASCII.GetBytes(pack.data);
            Send(data, pack.address.ToString(), pack.port);
        }
        public void Send(byte[] data, string address, int port)
        {
            Console.Write("Sending data... ");
            _server.Send(data, data.Length, address, port);
            Console.WriteLine("sent {0} bytes", data.Length);
        }

        public UDPPacket Receive()
        {
            var remoteEP = new IPEndPoint(IPAddress.Any, _port);
            Console.Write("Waiting for data... ");
            var buffer = _server.Receive(ref remoteEP);
            Console.WriteLine("received {0} bytes... done", buffer.Length);
            string bufferAsString = Encoding.ASCII.GetString(buffer, 0, buffer.Length);
            return new UDPPacket(bufferAsString, remoteEP.Address, remoteEP.Port);
        }
    }
}