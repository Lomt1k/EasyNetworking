using System;
using EasyNetworking.Messages;
using UnityEngine;

namespace EasyNetworking
{
    public class ServerMessageRegistrator : MonoBehaviour
    {
        private static void RegisterMessage(ServerMessage serverMessage, Type[] parameterTypes)
        {
            MessageRegistrator.RegisterMessage(serverMessage, parameterTypes);
        }
        
        public static void RegisterAllServerMessages()
        {
            //RegisterMessage(ServerMessage.Unknown.NewMessage, new[] {typeof(int), typeof(float) });
        }
    }
}
