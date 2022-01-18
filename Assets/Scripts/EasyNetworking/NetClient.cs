using System;
using UnityEngine;
using System.Net.Sockets;

namespace EasyNetworking
{
    public class NetClient : MonoBehaviour
    {
        private const int handleStreamDelay = 10;
        
        [SerializeField] private string _host = "127.0.0.1";
        [SerializeField] private int _port = 7777;
        
        private readonly TcpClient _tcpClient = new TcpClient();
        private StreamHandler _streamHandler;
    
        public bool isConnected => _tcpClient.Connected;
        
        [ContextMenu(nameof(ConnectToServer))]
        public void ConnectToServer()
        {
            Debug.Log($"{name} | Connecting to server...");
            TryToConnect();
        }
    
        private async void TryToConnect()
        {
            try
            {
                await _tcpClient.ConnectAsync(_host, _port);
                if (isConnected)
                {
                    OnConnectedToServer();
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"{name} | Connection error: {ex.Message}");
            }
        }
    
        private void OnConnectedToServer()
        {
            Debug.Log($"{name} | Connected to server. Connection state: {isConnected}");
            _streamHandler = new StreamHandler(_tcpClient.GetStream(), handleStreamDelay);
            _streamHandler.onCommandReceived += OnCommandReceived;
        }
    
        private void OnCommandReceived(ushort commandId)
        {
            Debug.Log($"{name} | recieved command from server (ID: {commandId})");
        }
    
        private void OnDestroy()
        {
            _tcpClient.Dispose();
        }
    }
}

