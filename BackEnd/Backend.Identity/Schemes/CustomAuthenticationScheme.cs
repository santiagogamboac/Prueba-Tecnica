using Backend.Application.Contracts.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Backend.Identity.Schemes
{
    public class CustomAuthenticationScheme : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string CustomScheme = nameof(CustomAuthenticationScheme);
        private readonly IAuthService _authService;

        public CustomAuthenticationScheme(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IServiceProvider serviceProvider
            ) : base(options, logger, encoder, clock)
        {
            var _scope = serviceProvider.CreateScope();
            _authService = _scope.ServiceProvider.GetRequiredService<IAuthService>();
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Context.Request.Query.TryGetValue("access_token", out StringValues accessToken))
                return Task.FromResult(AuthenticateResult.Fail("access token not found"));

            var validatedToken = _authService.ValidateToken(accessToken);
            if (validatedToken == null)
                return Task.FromResult(AuthenticateResult.Fail("token invalid"));
            var user = validatedToken.Value.Item1;
            var userId = validatedToken.Value.Item2.ToString();
            var claims = new Claim[] {
                new("UserId", userId),
                new(ClaimTypes.Name, user)
            };
            var identity = new ClaimsIdentity(claims, CustomScheme);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, new(), CustomScheme);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
