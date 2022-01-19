
namespace  EasyNetworking.Messages
{
    public readonly struct MessageData
    {
        public readonly ushort messageId;
        public readonly object[] parameters;

        public MessageData(ushort messageId, object[] parameters)
        {
            this.messageId = messageId;
            this.parameters = parameters;
        }
    }
}
