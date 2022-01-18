using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

namespace EasyNetworking
{
    public class StreamHandler
    {
        private NetworkStream _stream;
        private static Queue<byte[]> _messagesToSendQueue = new Queue<byte[]>();
        private int _streamHandleDelay;
    
        public Action<ushort> onCommandReceived;
        
        public StreamHandler(NetworkStream stream, int streamHandleDelay)
        {
            _stream = stream;
            _streamHandleDelay = streamHandleDelay;
            
            HandleStream();
        }

        public void AddMessageToSend(byte[] byteMessage)
        {
            _messagesToSendQueue.Enqueue(byteMessage);
        }
    
        private async void HandleStream()
        {
            while (true)
            {
                await Task.Delay(_streamHandleDelay);
                HandleReceivedData();
                HandleDataToSend();
            }
        }
    
        private void HandleReceivedData()
        {
            while (_stream.DataAvailable)
            {
                ParseNextCommand();
            }
        }
    
        private void ParseNextCommand()
        {
            try
            {
                ushort commandID = ParseMessageID();
                Debug.Log($"ParseMessageID {commandID}");
                //TODO
                onCommandReceived?.Invoke(commandID);
            }
            catch (Exception ex)
            {
                Debug.LogError("ParseNextCommand Exception: " + ex);
            }
        }
    
        private ushort ParseMessageID()
        {
            byte[] byteArr =
            {
                (byte) _stream.ReadByte(),
                (byte) _stream.ReadByte()
            };
            ushort commandID = BitConverter.ToUInt16(byteArr, 0);
            return commandID;
        }

        private void HandleDataToSend()
        {
            int currentOffset = 0;
            while (_messagesToSendQueue.Count > 0)
            {
                var nextMessage = _messagesToSendQueue.Dequeue();
                int bufferSize = nextMessage.Length;
                _stream.Write(nextMessage, currentOffset, bufferSize);
                currentOffset += bufferSize;
            }
            _stream.Flush();
        }
        

        ~StreamHandler()
        {
            onCommandReceived = null;
        }
        
    }
}
