using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

namespace EasyNetworking.Server
{
    public class NetServer : MonoBehaviour
    {
        private const int handleConnectionsMillisecondsDelay = 1000;
        
        [SerializeField] private int _port = 7777;

        private TcpListener _tcpListener;

        private Dictionary<TcpClient, ServerStreamHandler> _clients = new Dictionary<TcpClient, ServerStreamHandler>();
        
        [ContextMenu(nameof(StartListener))]
        public void StartListener()
        {
            _tcpListener = new TcpListener(IPAddress.Any, _port);
            _tcpListener.Start();
            Debug.Log($"NetServer | Server started on port: {_port}");

            AcceptConnections();
            HandleLostConnections();
        }
        
        private async void AcceptConnections()
        {
            Debug.Log($"{name} | Waiting for connections...");
            while (Application.isPlaying)
            {
                var newClient = await _tcpListener.AcceptTcpClientAsync();
                var networkStream = newClient.GetStream();
                var streamHandler = new ServerStreamHandler(networkStream);
                _clients.Add(newClient, streamHandler);
                
                var endPoint = newClient.Client.RemoteEndPoint;
                string formattedEndPoint = endPoint is IPEndPoint ipEndPoint
                    ? $"IP: {ipEndPoint.Address} Port: {ipEndPoint.Port}"
                    : endPoint.ToString();
                Debug.Log($"{name} | New connection from ({formattedEndPoint}) | Total connections: {_clients.Count}");
            }
        }

        private async void HandleLostConnections()
        {
            while (true)
            {
                await Task.Delay(handleConnectionsMillisecondsDelay);
                List<TcpClient> clientsToRemove = new List<TcpClient>();
                foreach (var client in _clients.Keys)
                {
                    if (!client.Connected)
                    {
                        clientsToRemove.Add(client);
                    }
                }

                foreach (var clientToRemove in clientsToRemove)
                {
                    Debug.Log($"Client disconnected {clientToRemove.Client.RemoteEndPoint}");
                    _clients.Remove(clientToRemove);
                }
            }
        }

        private void OnDestroy()
        {
            _tcpListener?.Stop();
        }
    }
}
