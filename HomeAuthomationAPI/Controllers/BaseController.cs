using HomeAuthomationAPI.Data;
using HomeAuthomationAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomeAuthomationAPI.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly HomeAutomationContext _context;

        protected BaseController(HomeAutomationContext context)
        {
            _context = context;
        }

        protected int? CurrentUserId => null;

        protected bool IsGlobalAdmin => true;

        protected Task<User?> GetCurrentUserAsync() => Task.FromResult<User?>(null);
    }
}
