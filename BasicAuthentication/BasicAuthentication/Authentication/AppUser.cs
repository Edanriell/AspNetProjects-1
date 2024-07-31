using Microsoft.AspNetCore.Identity;

namespace BasicAuthentication.Authentication;

// The IdentityUser class already contains the properties that we need to represent a user
// for most of the scenarios, such as UserName, Email, PasswordHash, PhoneNumber,
// and others.
public class AppUser : IdentityUser
{
    // public string FirstName { get; set; }
    // public string LastName { get; set; }
    // public string ProfilePicture { get; set; }
}