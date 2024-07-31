namespace BasicEfCoreReverseEngineering.Models;

public class Contact
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Title { get; set; }

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public virtual Address? Address { get; set; }
}