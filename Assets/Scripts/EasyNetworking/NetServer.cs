using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using EasyNetworking.Messages;
using UnityEngine;

namespace EasyNetworking
{
    public class NetServer : MonoBehaviour
    {
        private const int handleStreamDelay = 10;
        
        [SerializeField] private int _port = 7777;

        private TcpListener _tcpListener;

        private Dictionary<TcpClient, StreamHandler> _clients = new Dictionary<TcpClient, StreamHandler>();
        
        [ContextMenu(nameof(StartListener))]
        public void StartListener()
        {
            Debug.Log($"{name} | Starting server...");
            _tcpListener = new TcpListener(IPAddress.Any, _port);
            _tcpListener.Start();
            Debug.Log($"NetServer | Server started on port: {_port}");

            AcceptingClients();
            HandleConnections();
        }
        
        private async void AcceptingClients()
        {
            Debug.Log($"{name} | AcceptingClients...");
            while (true)
            {
                var newClient = await _tcpListener.AcceptTcpClientAsync();
                var streamHandler = new StreamHandler(newClient.GetStream(), handleStreamDelay);
                _clients.Add(newClient, streamHandler);
                
                var endPoint = newClient.Client.RemoteEndPoint;
                string formattedEndPoint = endPoint is IPEndPoint ipEndPoint
                    ? $"IP: {ipEndPoint.Address} Port: {ipEndPoint.Port}"
                    : endPoint.ToString();
                Debug.Log($"{name} | New connection from ({formattedEndPoint}) | Total connections: {_clients.Count}");
                
                //test
                // var stream = newClient.GetStream();
                // ushort messageID = 2236;
                // byte[] id = BitConverter.GetBytes(messageID);
                // stream.Write(id, 0, id.Length);
                // stream.Flush();
            }
        }

        private async void HandleConnections()
        {
            while (true)
            {
                await Task.Delay(handleStreamDelay);
                List<TcpClient> clientsToRemove = new List<TcpClient>();
                foreach (var client in _clients.Keys)
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
            //var stream = client.GetStream();
            // while (stream.DataAvailable)
            // {
            //     
            // }
        }

        private void OnDestroy()
        {
            _tcpListener?.Stop();
        }
    }
}
