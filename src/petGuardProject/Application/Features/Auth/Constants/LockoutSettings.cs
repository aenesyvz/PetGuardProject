namespace Application.Features.Auth.Constants;

public class LockoutSettings
{
    public int MaxFailedAccessAttempts { get; set; }
    public int LockoutDurationInMinutes { get; set; }
}