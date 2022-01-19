using System;
using EasyNetworking.Messages;
using UnityEngine;

namespace EasyNetworking
{
    public class ClientMessageRegistrator : MonoBehaviour
    {
        private static void RegisterMessage(ClientMessage clientMessage, Type[] parameterTypes)
        {
            MessageRegistrator.RegisterMessage(clientMessage, parameterTypes);
        }
        
        public static void RegisterAllClientMessages()
        {
            RegisterMessage(ClientMessage.SendValuesTest, new[] {typeof(int), typeof(float) });
            RegisterMessage(ClientMessage.SendHelloWorld, new[] {typeof(string) });
        }
    }
}

