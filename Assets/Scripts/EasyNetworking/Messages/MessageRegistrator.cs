using System;

namespace EasyNetworking.Messages
{
    public readonly struct ParameterTypes
    {
        public readonly Type[] types;
        public ParameterTypes(Type[] types)
        {
            this.types = types;
        }
    }
    
    
    public static class MessageRegistrator
    {
        private static ParameterTypes[] parameterTypesByClientMessageId;
        private static ParameterTypes[] parameterTypesByServerMessageId;

        static MessageRegistrator()
        {
            FillEmptyParamaters();
            RegisterAllMessages();
        }

        private static void FillEmptyParamaters()
        {
            parameterTypesByClientMessageId = new ParameterTypes[ushort.MaxValue];
            for (int i = 0; i < parameterTypesByClientMessageId.Length; i++)
            {
                parameterTypesByClientMessageId[i] = new ParameterTypes(Type.EmptyTypes);
            }
            
            parameterTypesByServerMessageId = new ParameterTypes[ushort.MaxValue];
            for (int i = 0; i < parameterTypesByServerMessageId.Length; i++)
            {
                parameterTypesByServerMessageId[i] = new ParameterTypes(Type.EmptyTypes);
            }
        }

        public static Type[] GetMessageParameters(MessageType messageType, ushort messageId)
        {
            return messageType == MessageType.MessageFromClient
                ? parameterTypesByClientMessageId[messageId].types
                : parameterTypesByServerMessageId[messageId].types;
        }

        public static void RegisterMessage(ClientMessage clientMessageId, Type[] parameterTypes)
        {
            parameterTypesByClientMessageId[(ushort)clientMessageId] = new ParameterTypes(parameterTypes);
        }
        
        public static void RegisterMessage(ServerMessage servertMessageId, Type[] parameterTypes)
        {
            parameterTypesByServerMessageId[(ushort)servertMessageId] = new ParameterTypes(parameterTypes);
        }
        
        private static void RegisterAllMessages()
        {
            ClientMessageRegistrator.RegisterAllClientMessages();
            ServerMessageRegistrator.RegisterAllServerMessages();
        }
        
        
    }
}
