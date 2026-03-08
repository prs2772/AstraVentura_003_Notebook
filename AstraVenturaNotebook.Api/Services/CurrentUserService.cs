using System.Security.Claims;
using AstraVenturaNotebook.Application.Interfaces;

namespace AstraVenturaNotebook.API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            // Extraemos el ID del usuario directamente del token validado por .NET
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User context is missing or invalid.");
            }

            return userId;
        }
    }
}
