using System;
namespace ArchaismDictionaryAndroidApp.Network
{
    interface INetworkConnection
    {
        bool isConnected { get; }
        void CheckConnection();
    }
}