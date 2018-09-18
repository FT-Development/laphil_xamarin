using System;


namespace LAPhil.Connectivity
{
    public interface IConnectivityDriver
    {
        bool IsConnected { get; }
    }
}
