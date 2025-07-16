using HomeAuthomationAPI.Data;
using HomeAuthomationAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HomeAuthomationAPI.Controllers
{
    [Authorize]
    public abstract class BaseController : ControllerBase
    {
        protected readonly HomeAutomationContext _context;

        protected BaseController(HomeAutomationContext context)
        {
            _context = context;
        }

        protected int? CurrentUserId
        {
            get
            {
                var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return int.TryParse(idClaim, out var id) ? id : null;
            }
        }

        protected bool IsGlobalAdmin => User.IsInRole("GlobalAdmin");

        protected async Task<User?> GetCurrentUserAsync()
        {
            var id = CurrentUserId;
            if (id == null) return null;
            return await _context.Users.FindAsync(id.Value);
        }
    }
}
