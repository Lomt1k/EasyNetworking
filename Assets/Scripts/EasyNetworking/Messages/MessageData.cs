
namespace  EasyNetworking.Messages
{
    public readonly struct MessageData
    {
        public readonly MessageId messageId;
        public readonly object[] parameters;

        public MessageData(MessageId messageId, object[] parameters)
        {
            this.messageId = messageId;
            this.parameters = parameters;
        }
    }
}
