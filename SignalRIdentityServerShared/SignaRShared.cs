using System.IO;
using System.Reflection;

namespace SignalRIdentityServerShared
{
    public interface IClient
    {
        Task ReceiveMessage(string sender, string message);
    }

    public interface IHubContract
    {
        Task Broadcast(string sender, string message);
    }
}