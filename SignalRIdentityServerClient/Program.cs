using IdentityModel.Client;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using SignalRIdentityServerShared;
using TypedSignalR.Client;

namespace SignalRIdentityServerClient
{
    public class WebClient : IClient, IHubConnectionObserver, IDisposable
    {
        private readonly IHubContract _hub;
        private readonly IDisposable _subscription;
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        public WebClient(HubConnection connection)
        {
            _hub = connection.CreateHubProxy<IHubContract>(_cancellationTokenSource.Token);
            _subscription = connection.Register<IClient>(this);
        }

        Task IClient.ReceiveMessage(string sender, string message)
        {
            Console.WriteLine("[{0}] {1}", sender, message);
            return Task.CompletedTask;
        }

        public Task OnClosed(Exception e)
        {
            Console.WriteLine($"[On Closed!]");
            return Task.CompletedTask;
        }

        public Task OnReconnected(string connectionId)
        {
            Console.WriteLine($"[On Reconnected!]");
            return Task.CompletedTask;
        }

        public Task OnReconnecting(Exception exception)
        {
            Console.WriteLine($"[On Reconnecting!]");
            return Task.CompletedTask;
        }

        public Task RequestBroadcast()
        {
            return _hub.Broadcast("Client", "Hello Server !");
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            var httpClient = new HttpClient();
            var discoveryDocument = httpClient.GetDiscoveryDocumentAsync("http://localhost:5155").Result;
            var tokenResponse = httpClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Address = discoveryDocument.TokenEndpoint,
                    ClientId = "client",
                    ClientSecret = "Prevo100",
                    Scope = "prevo100-api"
                }).Result;

            Console.WriteLine($"Token : {tokenResponse.AccessToken}");

            var url = "http://localhost:5155/Prevo100";

            HubConnection connection = new HubConnectionBuilder()
                .WithUrl(url, options =>
                {
                    options.Transports = HttpTransportType.WebSockets;
                    options.AccessTokenProvider = () => Task.FromResult(tokenResponse.AccessToken);
                })
                .WithAutomaticReconnect()
                .Build();

            var client = new WebClient(connection);

            connection.StartAsync().Wait();

            client.RequestBroadcast().Wait();

            connection.StopAsync().Wait();
            client.Dispose();
        }
    }
}