using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyNetworking.Messages
{
    public static class ClientMessageSender
    {
        public static void SendToServer(MessageId messageId, IComparable[] parameters)
        {
            var byteList = new List<byte>();
            
            var arrayId = BitConverter.GetBytes((ushort)messageId);
            byteList.AddRange(arrayId);
            
            foreach (var parameter in parameters)
            {
                var byteArr = GetByteArray(parameter);
                byteList.AddRange(byteArr);
            }
            byte[] byteMessage = byteList.ToArray();
            
            var streamHandler = NetClient.instance.streamHandler;
            streamHandler?.AddMessageToSend(byteMessage);
        }
        
        private static IEnumerable<byte> GetByteArray(IComparable value)
        {
            switch (value)
            {
                case int var: return BitConverter.GetBytes(var);
                case uint var: return BitConverter.GetBytes(var);
                case float var: return BitConverter.GetBytes(var);
                case bool var: return BitConverter.GetBytes(var);
                case short var: return BitConverter.GetBytes(var);
                case ushort var: return BitConverter.GetBytes(var);
                case char var: return BitConverter.GetBytes(var);
                case byte var: return BitConverter.GetBytes(var);
                case long var: return BitConverter.GetBytes(var);
                case ulong var: return BitConverter.GetBytes(var);
                case double var: return BitConverter.GetBytes(var);
                default:
                    Debug.LogError($"ClientMessageSender | Отправка переменной типа {value.GetType()} не поддерживается");
                    return null;
            }
        }
        
        private static int GetSize(IComparable value)
        {
            switch (value)
            {
                case int var: return 4;
                case uint var: return 4;
                case float var: return 4;
                case bool var: return 1;
                case short var: return 2;
                case ushort var: return 2;
                case char var: return 1;
                case byte var: return 1;
                case long var: return 8;
                case ulong var: return 8;
                case double var: return 8;
                default:
                    Debug.LogError($"ClientMessageSender | Отправка переменной типа {value.GetType()} не поддерживается");
                    return 0;
            }
        }
        
        
    }
}

