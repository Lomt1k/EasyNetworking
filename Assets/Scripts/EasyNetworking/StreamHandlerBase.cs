using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using EasyNetworking.Messages;
using UnityEngine;

namespace EasyNetworking
{
    public abstract class StreamHandlerBase
    {
        private const int milisecondsBeforeSendingData = 1;
        
        private NetworkStream _stream;
        private BinaryReader _reader;
        private BinaryWriter _writer;
        
        private Queue<MessageData> _messagesToSendQueue = new Queue<MessageData>();

        protected StreamHandlerBase(NetworkStream stream)
        {
            _stream = stream;
            _reader = new BinaryReader(_stream);
            _writer = new BinaryWriter(_stream);
            
            HandleReceivedData();
            HandleDataSending();
        }

        public void AddMessageToSend(MessageData messageData)
        {
            _messagesToSendQueue.Enqueue(messageData);
        }

        private async void HandleReceivedData()
        {
            byte[] messageIdArr = new byte[2];
            while (Application.isPlaying)
            {
                await _stream.ReadAsync(messageIdArr, 0, 2);
                ushort messageId = BitConverter.ToUInt16(messageIdArr, 0);
                while (_stream.DataAvailable)
                {
                    HandleNextReceivedMessage(messageId);
                }
            }
        }

        private void HandleNextReceivedMessage(ushort messageId)
        {
            var parameterTypes = GetReceivedMessageParameterTypes(messageId);

            object[] parameters = new object[parameterTypes.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                parameters[i] = StreamDataHelper.ReadNextParameter(_reader, parameterTypes[i]);
            }
            
            ExecuteReceivedMessage(messageId, parameters);
        }

        private async void HandleDataSending()
        {
            while (Application.isPlaying)
            {
                await Task.Delay(milisecondsBeforeSendingData);
                HandleMessagesToSend();
            }
        }

        private void HandleMessagesToSend()
        {
            while (_messagesToSendQueue.Count > 0)
            {
                var messageData = _messagesToSendQueue.Dequeue();
                WriteMessageToStream(messageData);
            }
        }

        private void WriteMessageToStream(MessageData messageData)
        {
            _writer.Write(messageData.messageId);
            foreach (var parameter in messageData.parameters)
            {
                StreamDataHelper.Write(_writer, parameter);
            }
            _writer.Flush();
        }

        

        protected abstract Type[] GetReceivedMessageParameterTypes(ushort messageId);
        protected abstract void ExecuteReceivedMessage(ushort messageId, object[] parameters);


        ~StreamHandlerBase()
        {
        }
        
    }
}
