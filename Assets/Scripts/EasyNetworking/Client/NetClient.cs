using System;
using UnityEngine;
using System.Net.Sockets;
using EasyNetworking.Messages;

namespace EasyNetworking.Client
{
    public class NetClient : MonoBehaviour
    {
        private const int handleStreamDelay = 10;
        
        [SerializeField] private string _host = "127.0.0.1";
        [SerializeField] private int _port = 7777;

        public static NetClient instance;
        
        private readonly TcpClient _tcpClient = new TcpClient();
        private ClientStreamHandler _streamHandler;
    
        public bool isConnected => _tcpClient.Connected;

        private void Awake()
        {
            instance = this;
        }

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
            var networkStream = _tcpClient.GetStream();
            _streamHandler = new ClientStreamHandler(networkStream, handleStreamDelay);
        }
        
        public void SendToServer(MessageId messageId, object[] parameters)
        {
            var messageData = new MessageData(messageId, parameters); 
            _streamHandler.AddMessageToSend(messageData);
        }

        [ContextMenu(nameof(SendValuesTest))]
        public void SendValuesTest()
        {
            int intValue = 1122;
            float floatValue = 100.5f;
            SendToServer(MessageId.SendValuesTest, new object[]{intValue, floatValue} );
        }
        
        [ContextMenu(nameof(SendHelloToServer))]
        public void SendHelloToServer()
        {
            string helloString = "Hello world!";
            SendToServer(MessageId.SendHelloWorld, new object[]{helloString} );
        }
    
        private void OnDestroy()
        {
            _tcpClient.Dispose();
        }
    }
}

