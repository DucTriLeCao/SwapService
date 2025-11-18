using System.Security.Claims;
using SwapService.Application.DTOs;

namespace SwapService.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static StaffContext GetStaffContext(this ClaimsPrincipal user)
    {
        var staffIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                          ?? user.FindFirst("sub")?.Value;
        var stationIdClaim = user.FindFirst("stationId")?.Value;
        var nameClaim = user.FindFirst(ClaimTypes.Name)?.Value 
                       ?? user.FindFirst("name")?.Value;

        if (string.IsNullOrEmpty(staffIdClaim))
            throw new UnauthorizedAccessException("Staff ID not found in token");

        if (string.IsNullOrEmpty(stationIdClaim))
            throw new UnauthorizedAccessException("Station ID not found in token");

        return new StaffContext
        {
            StaffId = Guid.Parse(staffIdClaim),
            StationId = Guid.Parse(stationIdClaim),
            StaffName = nameClaim ?? "Unknown"
        };
    }
}
