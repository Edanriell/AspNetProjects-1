using Grpc.Core;
using Grpc.Net.Client;

namespace Client;

internal class BidirectionalStreamingClient
{
    public async Task SendMessage()
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:5013");
        var client = new Chat.ChatClient(channel);

        // Create a streaming request
        using var streamingCall = client.SendMessage();
        Console.WriteLine("Starting background task to receive messages...");
        var responseReaderTask = Task.Run(async () =>
        {
            await foreach (var response in streamingCall.ResponseStream.ReadAllAsync())
                Console.WriteLine(response.Message);
        });

        Console.WriteLine("Starting to send messages...");
        Console.WriteLine("Type a message then press enter.");
        while (true)
        {
            var message = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(message)) break;
            await streamingCall.RequestStream.WriteAsync(new ChatMessage
            {
                Message = message
            });
        }

        Console.WriteLine("Disconnecting...");
        await streamingCall.RequestStream.CompleteAsync();
        await responseReaderTask;
    }
}

//  Because we use a console application to call the bidirectional streaming service, we need to use
//  a background task to read the stream response messages. The ReadAllAsync() method
//  returns an IAsyncEnumerable<T> object, which can be iterated over using the await
//  foreach statement. In the background task, we use the await foreach statement to
//  iterate over the stream response messages and print them to the console.
//  Additionally, we use a while loop to read the input from the console and send the stream
//  request messages to the server in the main thread. The while loop ends when the user enters an
//  empty string. At the end of the method, we call the RequestStream.CompleteAsync()
//  method to indicate that the stream request message is complete so that the server can finish
//  processing the stream request messages gracefully