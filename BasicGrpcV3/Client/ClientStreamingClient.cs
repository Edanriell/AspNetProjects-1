using Grpc.Net.Client;

namespace Client;

internal class ClientStreamingClient
{
    public async Task SendRandomNumbers()
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:5013");
        var client = new RandomNumbers.RandomNumbersClient(channel);

        // Create a streaming request
        using var clientStreamingCall = client.SendRandomNumbers();
        var random = new Random();
        for (var i = 0; i < 20; i++)
        {
            await clientStreamingCall.RequestStream.WriteAsync(new SendRandomNumbersRequest
            {
                Number = random.Next(1, 100)
            });
            await Task.Delay(1000);
        }

        await clientStreamingCall.RequestStream.CompleteAsync();

        // Get the response
        var response = await clientStreamingCall;
        Console.WriteLine($"Count: {response.Count}, Sum: {response.Sum}");
        Console.ReadKey();
    }
}

//  In the SendRandomNumbers() method, we create an AsyncClientStreamingCall object
//  by calling the SendRandomNumbers() method of the RandomNumbersClient class. Note that
//  the client streaming call starts when the SendRandomNumbers() method is called, but the client
//  does not send any messages until the RequestStream.CompleteAsync() method is called.
//  In a for loop, we use the RequestStream.WriteAsync() method to send the stream request
//  message to the server. At the end of the method, we call the RequestStream.CompleteAsync()
//  method to indicate that the stream request message is complete. The stream request message contains
//  20 numbers, which are generated randomly