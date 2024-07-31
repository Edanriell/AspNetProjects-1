using Grpc.Core;

namespace BasicGrpcV2.Services;

public class ContactService(ILogger<ContactService> logger) : Contact.ContactBase
{
    private readonly ILogger<ContactService> _logger = logger;

    public override Task<CreateContactResponse> CreateContact(CreateContactRequest request, ServerCallContext context)
    {
        return Task.FromResult(new CreateContactResponse
        {
            ContactId = Guid.NewGuid().ToString()
        });
    }
}