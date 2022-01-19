using System;
using System.Net.Sockets;
using EasyNetworking.Messages;
using UnityEngine;

namespace EasyNetworking.Client
{
    public class ClientStreamHandler : StreamHandlerBase
    {
        public ClientStreamHandler(NetworkStream stream, int streamHandleDelay) : base(stream, streamHandleDelay)
        {
        }

        protected override Type[] GetMessageParameterTypes(ushort messageId)
        {
            return MessageDictionary.GetMessage((MessageId)messageId).parameterTypes;
        }

        protected override void ExecuteReceivedMessage(ushort messageId, object[] parameters)
        {
            var messId = (MessageId)messageId;
            string log = $"ClientStreamHandler | Received {messId} with parameters: ";
            foreach (var parameter in parameters)
            {
                log += parameter + " ";
            }
            Debug.Log(log);
        }
        
    }
}

