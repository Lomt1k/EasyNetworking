using System;
using System.Collections.Generic;

namespace EasyNetworking.Messages
{
    public static class MessageDictionary
    {
        private static readonly Dictionary<MessageId, MessageBase> allMessages;

        static MessageDictionary()
        {
            allMessages = new Dictionary<MessageId, MessageBase>();
            RegisterAllMessages();
        }

        public static MessageBase GetMessage(MessageId messageId)
        {
            return allMessages.ContainsKey(messageId) ? allMessages[messageId] : null;
        }

        private static void RegisterMessage(MessageId messageId, Type[] parameterTypes)
        {
            var message = new MessageWithParameters(parameterTypes);
            allMessages.Add(messageId, message);
        }
        
        private static void RegisterAllMessages()
        {
            RegisterMessage(MessageId.SendValuesTest, new[] {typeof(int), typeof(float) });
        }
    }
    
}
