using Grpc.Core;

namespace BasicGrpcV3.Services;

public class RandomNumbersService(ILogger<RandomNumbersService> logger) : RandomNumbers.RandomNumbersBase
{
    public override async Task GetRandomNumbers(GetRandomNumbersRequest request,
        IServerStreamWriter<GetRandomNumbersResponse> responseStream, ServerCallContext context)
    {
        var random = new Random();
        for (var i = 0; i < request.Count; i++)
        {
            await responseStream.WriteAsync(new GetRandomNumbersResponse
            {
                Number = random.Next(request.Min, request.Max)
            });
            await Task.Delay(1000);
        }
    }

    //  In the implementation of the GetRandomNumbers() method, we use a for loop to
    //  generate random numbers and send them to the client every second. Note that we use the
    //  responseStream.WriteAsync() method to send the stream response message to the
    //  client. The message finishes sending when the loop ends.

    // The following code continues to stream random numbers until the client cancels the request.
    //public override async Task GetRandomNumbers(GetRandomNumbersRequest request,
    //    IServerStreamWriter<GetRandomNumbersResponse> responseStream, ServerCallContext context)
    //{
    //    var random = new Random();
    //    while (!context.CancellationToken.IsCancellationRequested)
    //    {
    //        await responseStream.WriteAsync(new GetRandomNumbersResponse
    //        {
    //            Number = random.Next(request.Min, request.Max)
    //        });
    //        await Task.Delay(1000, context.CancellationToken);
    //    }
    //}

    public override async Task<SendRandomNumbersResponse> SendRandomNumbers(
        IAsyncStreamReader<SendRandomNumbersRequest> requestStream, ServerCallContext context)
    {
        var count = 0;
        var sum = 0;
        await foreach (var request in requestStream.ReadAllAsync())
        {
            logger.LogInformation($"Received: {request.Number}");
            count++;
            sum += request.Number;
        }

        return new SendRandomNumbersResponse
        {
            Count = count,
            Sum = sum
        };
    }
}

//  We utilize the IAsyncStreamReader<T>.ReadAllAsync() method in the preceding code
//  to read all the stream request messages from the client. Subsequently, we use await foreach to
//  iterate over the stream request messages. Lastly, we compute the count and sum of the numbers and
//  return a SendRandomNumbersResponse object