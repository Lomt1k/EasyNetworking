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
        private static ParameterTypes[] parameterTypesByMessageId;

        static MessageRegistrator()
        {
            FillEmptyParamaters();
            RegisterAllMessages();
        }

        private static void FillEmptyParamaters()
        {
            parameterTypesByMessageId = new ParameterTypes[ushort.MaxValue];
            for (int i = 0; i < parameterTypesByMessageId.Length; i++)
            {
                parameterTypesByMessageId[i] = new ParameterTypes(Type.EmptyTypes);
            }
        }

        public static Type[] GetMessageParameters(ushort messageId)
        {
            return parameterTypesByMessageId[messageId].types;
        }

        private static void RegisterMessage(MessageId messageId, Type[] parameterTypes)
        {
            parameterTypesByMessageId[(ushort)messageId] = new ParameterTypes(parameterTypes);
        }
        
        private static void RegisterAllMessages()
        {
            RegisterMessage(MessageId.SendValuesTest, new[] {typeof(int), typeof(float) });
            RegisterMessage(MessageId.SendHelloWorld, new[] {typeof(string) });
        }
        
    }

}
