using Grpc.Net.Client;

namespace Client;

internal class InvoiceClient
{
    public async Task CreateContactAsync()
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:5013");
        var client = new Contact.ContactClient(channel);
        var reply = await client.CreateContactAsync(new CreateContactRequest
        {
            Email = "abc@abc.com",
            FirstName = "John",
            LastName = "Doe",
            IsActive = true,
            Phone = "1234567890",
            YearOfBirth = 1980
        });
        Console.WriteLine("Created Contact: " + reply.ContactId);
        Console.ReadKey();
    }
}

//  A gRPC channel is used to establish a connection to the gRPC server on the specified address
//  and port. Once we have the gRPC channel, we can create an instance of the ContactClient
//  class, which is generated from the proto file. Then, we call the CreateContactAsync()
//  method to create a contact. The CreateContactAsync() method accepts a
//  CreateContactRequest object as the parameter. The CreateContactAsync()
//  method returns a CreateContactResponse object, which contains the ContactId
//  value. At the end of the method, we print the ContactId value to the console.
//  This method is straightforward. There are a few things to note:
//  Creating a gRPC channel is an expensive operation. So, it is recommended to reuse the
//  gRPC channel. However, a gRPC client is a lightweight object, so there is no need to reuse it.
//  You can create multiple gRPC clients from one gRPC channel, and you can safely use multiple
//  gRPC clients concurrently