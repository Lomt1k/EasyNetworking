using System;
using System.Net.Sockets;
using EasyNetworking.Messages;
using UnityEngine;

namespace EasyNetworking.Server
{
    public class ServerStreamHandler : StreamHandlerBase
    {
        public ServerStreamHandler(NetworkStream stream, int streamHandleDelay) : base(stream, streamHandleDelay)
        {
        }

        protected override Type[] GetReceivedMessageParameterTypes(ushort messageId)
        {
            return MessageRegistrator.GetMessageParameters(MessageType.MessageFromClient, messageId);
        }

        protected override void ExecuteReceivedMessage(ushort messageId, object[] parameters)
        {
            var messId = (ClientMessage)messageId;
            string log = $"ServerStreamHandler | Received {messId} with parameters: ";
            foreach (var parameter in parameters)
            {
                log += parameter + " ";
            }
            Debug.Log(log);
        }
        
    }
}