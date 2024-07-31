using Client;

Console.WriteLine("Hello, World!");

// var contactClient = new InvoiceClient();
// await contactClient.CreateContactAsync();

// var serverStreamingClient = new ServerStreamingClient();
// await serverStreamingClient.GetRandomNumbers();

// var clientStreamingClient = new ClientStreamingClient();
// await clientStreamingClient.SendRandomNumbers();

var bidirectionalStreamingClient = new BidirectionalStreamingClient();
await bidirectionalStreamingClient.SendMessage();