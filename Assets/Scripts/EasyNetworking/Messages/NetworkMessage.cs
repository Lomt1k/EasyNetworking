using System;

namespace EasyNetworking.Messages
{
    public class NetworkMessage
    {
        public Type[] parameterTypes { get; }

        public NetworkMessage(Type[] parameterTypes)
        {
            this.parameterTypes = parameterTypes;
        }
    }
}
