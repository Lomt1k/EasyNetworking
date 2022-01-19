using System;
using UnityEngine;

namespace EasyNetworking.Messages
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
