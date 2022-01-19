using System;
using System.Collections.Generic;

namespace EasyNetworking.Messages
{
    public static class MessageDictionary
    {
        private static readonly Dictionary<MessageId, NetworkMessage> allMessages;

        static MessageDictionary()
        {
            allMessages = new Dictionary<MessageId, NetworkMessage>();
            RegisterAllMessages();
        }

        public static NetworkMessage GetMessage(MessageId messageId)
        {
            return allMessages.ContainsKey(messageId) ? allMessages[messageId] : null;
        }

        private static void RegisterMessage(MessageId messageId, Type[] parameterTypes)
        {
            var message = new NetworkMessage(parameterTypes);
            allMessages.Add(messageId, message);
        }
        
        private static void RegisterAllMessages()
        {
            RegisterMessage(MessageId.SendValuesTest, new[] {typeof(int), typeof(float) });
            RegisterMessage(MessageId.SendHelloWorld, new[] {typeof(string) });
        }
    }
    
}
