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
        private NetworkStream _stream;
        private BinaryReader _reader;
        private BinaryWriter _writer;
        
        private Queue<MessageData> _messagesToSendQueue = new Queue<MessageData>();
        private int _streamHandleDelay;
        
        public StreamHandlerBase(NetworkStream stream, int streamHandleDelay)
        {
            _stream = stream;
            _reader = new BinaryReader(_stream);
            _writer = new BinaryWriter(_stream);
            _streamHandleDelay = streamHandleDelay;
            
            HandleReceivedData();
            HandleDataSending();
        }

        public void AddMessageToSend(MessageData messageData)
        {
            _messagesToSendQueue.Enqueue(messageData);
        }

        private async void HandleReceivedData()
        {
            while (true)
            {
                await Task.Delay(_streamHandleDelay);
                while (_stream.DataAvailable)
                {
                    HandleNextReceivedMessage();
                }
            }
        }

        private void HandleNextReceivedMessage()
        {
            ushort messageId = _reader.ReadUInt16();
            var parameterTypes = GetMessageParameterTypes(messageId);

            object[] parameters = new object[parameterTypes.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                parameters[i] = ReadNextParameter(parameterTypes[i]);
            }
            
            ExecuteReceivedMessage(messageId, parameters);
        }

        private object ReadNextParameter(Type parameterType)
        {
            switch (parameterType)
            {
                case Type _ when parameterType == typeof(int):
                    return _reader.ReadInt32();
                case Type _ when parameterType == typeof(uint):
                    return _reader.ReadUInt32();
                case Type _ when parameterType == typeof(float):
                    return _reader.ReadSingle();
                case Type _ when parameterType == typeof(bool):
                    return _reader.ReadBoolean();
                case Type _ when parameterType == typeof(short):
                    return _reader.ReadInt16();
                case Type _ when parameterType == typeof(ushort):
                    return _reader.ReadUInt16();
                case Type _ when parameterType == typeof(long):
                    return _reader.ReadInt64();
                case Type _ when parameterType == typeof(ulong):
                    return _reader.ReadUInt64();
                case Type _ when parameterType == typeof(string):
                    return _reader.ReadString();
                case Type _ when parameterType == typeof(char):
                    return _reader.ReadChar();
                case Type _ when parameterType == typeof(byte):
                    return _reader.ReadByte();
                case Type _ when parameterType == typeof(double):
                    return _reader.ReadDouble();
                case Type _ when parameterType == typeof(decimal):
                    return _reader.ReadDecimal();
            }
            
            Debug.LogError($"StreamHandler | Получено сообщение с параметром неизвестного типа");
            return null;
        }

        private async void HandleDataSending()
        {
            while (true)
            {
                await Task.Delay(_streamHandleDelay);
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
            _writer.Write((ushort)messageData.messageId);

            foreach (var parameter in messageData.parameters)
            {
                WriteToStream(parameter);
            }
            _writer.Flush();
        }

        private void WriteToStream(object variable)
        {
            switch (variable)
            {
                case int var: _writer.Write(var); return;
                case uint var: _writer.Write(var); return;
                case float var: _writer.Write(var); return;
                case bool var: _writer.Write(var); return;
                case short var: _writer.Write(var); return;
                case ushort var: _writer.Write(var); return;
                case long var: _writer.Write(var); return;
                case ulong var: _writer.Write(var); return;
                case string var: _writer.Write(var); return;
                case char var: _writer.Write(var); return;
                case byte var: _writer.Write(var); return;
                case double var: _writer.Write(var); return;
                case decimal var: _writer.Write(var); return;
            }
            Debug.LogError($"StreamHandler | Отправка переменной типа {variable.GetType()} не поддерживается");
        }

        protected abstract Type[] GetMessageParameterTypes(ushort messageId);
        protected abstract void ExecuteReceivedMessage(ushort messageId, object[] parameters);


        ~StreamHandlerBase()
        {
        }
        
    }
}
