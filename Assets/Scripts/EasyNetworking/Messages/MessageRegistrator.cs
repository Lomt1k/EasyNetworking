using System;
using System.Collections.Generic;

namespace EasyNetworking.Messages
{
    public static class MessageRegistrator
    {
        private static readonly Dictionary<ushort, Type[]> messageParameterTypesByMessageId;

        static MessageRegistrator()
        {
            messageParameterTypesByMessageId = new Dictionary<ushort, Type[]>();
            RegisterAllMessages();
        }

        public static Type[] GetMessageParameters(ushort messageId)
        {
            return messageParameterTypesByMessageId.ContainsKey(messageId) 
                ? messageParameterTypesByMessageId[messageId]
                : Type.EmptyTypes;
        }

        private static void RegisterMessage(MessageId messageId, Type[] parameterTypes)
        {
            messageParameterTypesByMessageId.Add((ushort)messageId, parameterTypes);
        }
        
        private static void RegisterAllMessages()
        {
            RegisterMessage(MessageId.SendValuesTest, new[] {typeof(int), typeof(float) });
            RegisterMessage(MessageId.SendHelloWorld, new[] {typeof(string) });
        }
    }
    
}
