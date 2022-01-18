using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

namespace EasyNetworking
{
    public class NetServer : MonoBehaviour
    {
        private const int handleStreamDelay = 10;
        
        [SerializeField] private int _port = 7777;

        private TcpListener _tcpListener;

        private List<TcpClient> _clients = new List<TcpClient>();
        
        [ContextMenu(nameof(StartListener))]
        public void StartListener()
        {
            Debug.Log($"{name} | Starting server...");
            _tcpListener = new TcpListener(IPAddress.Any, _port);
            _tcpListener.Start();
            Debug.Log($"NetServer | Server started on port: {_port}");

            AcceptingClients();
            HandleConnections();
            
            TestTypes();
        }
        
        private async void AcceptingClients()
        {
            Debug.Log($"{name} | AcceptingClients...");
            while (true)
            {
                var newClient = await _tcpListener.AcceptTcpClientAsync();
                _clients.Add(newClient);
                
                var endPoint = newClient.Client.RemoteEndPoint;
                string formattedEndPoint = endPoint is IPEndPoint ipEndPoint
                    ? $"IP: {ipEndPoint.Address} Port: {ipEndPoint.Port}"
                    : endPoint.ToString();
                Debug.Log($"{name} | New connection from ({formattedEndPoint}) | Total connections: {_clients.Count}");
                
                //test
                var stream = newClient.GetStream();
                ushort commandID = 2236;
                byte[] message = BitConverter.GetBytes(commandID);
                stream.Write(message, 0, message.Length);
                stream.Flush();
            }
        }

        private async void HandleConnections()
        {
            while (true)
            {
                await Task.Delay(handleStreamDelay);
                List<TcpClient> clientsToRemove = new List<TcpClient>();
                foreach (var client in _clients)
                {
                    if (!client.Connected)
                    {
                        clientsToRemove.Add(client);
                        continue;
                    }
                    HandleConnection(client);
                }

                foreach (var clientToRemove in clientsToRemove)
                {
                    Debug.Log($"Client disconnected {clientToRemove.Client.RemoteEndPoint}");
                    _clients.Remove(clientToRemove);
                }
            }
        }

        private void HandleConnection(TcpClient client)
        {
            var stream = client.GetStream();
            // while (stream.DataAvailable)
            // {
            //     
            // }
        }

        private void TestTypes()
        {
            ushort ushortValue = 300;
            int intValue = 223;
            Vector3 vector = Vector3.zero;
            IComparable[] parameters =
            {
                ushortValue,
                intValue,
            };
            
            switch (parameters[0])
            {
                case ushort uint16:
                    break;
            }

            int totalSize = 0;
            foreach (var parameter in parameters)
            {
                var size = GetSizeOfParameter(parameter);
                //var size = System.Runtime.InteropServices.Marshal.SizeOf(parameter);
                Debug.Log($"parameter {parameter.GetType().Name} | size: {size}");
                totalSize += size;
            }
            Debug.Log($"Total size: {totalSize}");
        }

        private int GetSizeOfParameter(IComparable parameter)
        {
            switch (parameter)
            {
                case ushort ushortVar:
                    return sizeof(ushort);
                case int intVar:
                    return sizeof(int);
            }

            return 0;
        }

        private void OnDestroy()
        {
            _tcpListener?.Stop();
        }
    }
}
