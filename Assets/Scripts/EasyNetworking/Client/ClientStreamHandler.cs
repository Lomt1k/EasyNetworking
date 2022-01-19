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

        protected override Type[] GetReceivedMessageParameterTypes(ushort messageId)
        {
            return MessageRegistrator.GetMessageParameters(MessageType.MessageFromServer, messageId);
        }

        protected override void ExecuteReceivedMessage(ushort messageId, object[] parameters)
        {
            var messId = (ServerMessage)messageId;
            string log = $"ClientStreamHandler | Received {messId} with parameters: ";
            foreach (var parameter in parameters)
            {
                log += parameter + " ";
            }
            Debug.Log(log);
        }
        
    }
}

