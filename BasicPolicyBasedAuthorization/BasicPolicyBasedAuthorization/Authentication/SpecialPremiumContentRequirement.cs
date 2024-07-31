using Microsoft.AspNetCore.Authorization;

namespace BasicPolicyBasedAuthorization.Authentication;

public class SpecialPremiumContentRequirement : IAuthorizationRequirement
{
    public SpecialPremiumContentRequirement(string country)
    {
        Country = country;
    }

    public string Country { get; }
}