using System;
using UnityEngine;
using System.Net.Sockets;
using EasyNetworking.Messages;

namespace EasyNetworking
{
    public class NetClient : MonoBehaviour
    {
        private const int handleStreamDelay = 10;
        
        [SerializeField] private string _host = "127.0.0.1";
        [SerializeField] private int _port = 7777;

        public static NetClient instance;
        
        private readonly TcpClient _tcpClient = new TcpClient();
        private StreamHandler _streamHandler;
    
        public bool isConnected => _tcpClient.Connected;
        public StreamHandler streamHandler => _streamHandler;

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
            _streamHandler = new StreamHandler(_tcpClient.GetStream(), handleStreamDelay);
            _streamHandler.onCommandReceived += OnCommandReceived;
        }
    
        private void OnCommandReceived(ushort commandId)
        {
            Debug.Log($"{name} | recieved command from server (ID: {commandId})");
        }
        
        public static void SendToServer(MessageId messageId, object[] parameters)
        {
            if (instance == null)
                Debug.LogError($"NetClient not initialized");
            
            var messageData = new MessageData(messageId, parameters); 
            instance.streamHandler.AddMessageToSend(messageData);
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

