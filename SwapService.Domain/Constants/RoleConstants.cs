namespace SwapService.Domain.Constants;

public static class RoleConstants
{
    public const string Admin = "Admin";
    public const string Staff = "Staff";
    public const string Driver = "Driver";

    public static readonly string[] All = { Admin, Staff, Driver };
}
