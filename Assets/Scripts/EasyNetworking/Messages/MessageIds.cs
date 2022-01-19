using System;

namespace EasyNetworking.Messages
{
    public enum MessageType : byte
    {
        Unknown = 0,
        MessageFromClient = 50,
        MessageFromServer = 100
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

