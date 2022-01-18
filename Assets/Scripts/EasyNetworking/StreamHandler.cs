using System;
using System.Net.Sockets;
using System.Threading.Tasks;

public class StreamHandler
{
    private NetworkStream _stream;
    private int _streamHandleDelay;

    public Action<ushort> onCommandReceived;
    
    public StreamHandler(NetworkStream stream, int streamHandleDelay)
    {
        _stream = stream;
        _streamHandleDelay = streamHandleDelay;
        
        HandleStream();
    }

    private async void HandleStream()
    {
        while (true)
        {
            await Task.Delay(_streamHandleDelay);
            HandleReceivedData();
        }
    }

    private void HandleReceivedData()
    {
        while (_stream.DataAvailable)
        {
            HandleNextCommandFromStream();
        }
    }

    private void HandleNextCommandFromStream()
    {
        ushort commandID = ReadCommandID();
        //TODO
        onCommandReceived?.Invoke(commandID);
    }

    private ushort ReadCommandID()
    {
        byte[] byteArr =
        {
            (byte) _stream.ReadByte(),
            (byte) _stream.ReadByte()
        };
        ushort commandID = BitConverter.ToUInt16(byteArr, 0);
        return commandID;
    }


    ~StreamHandler()
    {
        onCommandReceived = null;
    }
    
}
