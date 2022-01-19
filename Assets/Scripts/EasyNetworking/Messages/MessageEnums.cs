
namespace EasyNetworking.Messages
{
    public enum MessageType : byte
    {
        Unknown = 0,
        MessageFromClient = 1,
        MessageFromServer = 2
    }
    
    public enum ClientMessage : ushort
    {
        Unknown = 0,
        SendValuesTest,
        SendHelloWorld
    }

    public enum ServerMessage : ushort
    {
        Unknown = 0,
    }
}

