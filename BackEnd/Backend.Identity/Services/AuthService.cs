using Backend.Application.Contracts.Identity;
using Backend.Application.Models.Common;
using Backend.Application.Models.Identity;
using Backend.Identity.Models;
using Backend.Identity.Persistence;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Identity.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWTSettings _jwtSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ShopIdentityContext _context;
        private readonly TokenValidationParameters _tokenValidationParameters;
       // private readonly IFinanzautoDomainService _finanzautoDomainService;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JWTSettings> jwtSettings,
            ShopIdentityContext context,
            IHttpContextAccessor httpContextAccessor,
            TokenValidationParameters tokenValidationParameters            
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _tokenValidationParameters = tokenValidationParameters;
           // _finanzautoDomainService = finanzautoDomainService;
        }

        #region Login
        public async Task<AuthResponse> Login(AuthRequest request)
        {         
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
                return null; 

            var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!passwordValid)
                return null;

            var token = await GenerateToken(user);
            
            var authReponse = new AuthResponse
            {
                UserId = user.Id,
                Token = token.Item1,
                RefreshToken = token.Item2,
                UserName = request.UserName,
                FirstName = user.Name,
                LastName = user.LastName,      
            };
            return authReponse;
        }

        private async Task<Tuple<string, string>> GenerateToken(Models.ApplicationUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Key));

            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();
            foreach (var role in roles)
                roleClaims.Add(new Claim(ClaimTypes.Role, role));

            var claimsIdentity = new[]
            {
                new Claim("Id", user.Id),
                new Claim("UserName", user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }
            .Union(userClaims)
            .Union(roleClaims);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    claimsIdentity
                ),
                Expires = DateTime.UtcNow.Add(_jwtSettings.ExpireTime),
                SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                IsUsed = false,
                IsRevoked = false,
                UserId = user.Id,
                CreatedDate = DateTime.UtcNow,
                ExpireDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenDurationInDays),
                Token = $"{GenerateRandomTokenCharacters(_jwtSettings.RefreshTokenCharacterLenght)}{Guid.NewGuid()}"
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new Tuple<string, string>(jwtToken, refreshToken.Token);
        }

        private string GenerateRandomTokenCharacters(int lenght)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, lenght)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<AuthResponse> RefreshToken(TokenRequest request)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParamsClone = _tokenValidationParameters.Clone();
            tokenValidationParamsClone.ValidateLifetime = false;

            try
            {
                //validation: token format is correct
                var tokenVerification = jwtTokenHandler.ValidateToken(
                    request.Token,
                    tokenValidationParamsClone,
                    out var validatedToken);

                //validation: verifies encryption
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(
                        SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase);
                }

                //validation: token expiration date verification
                var utcExpiryDate = long.Parse(tokenVerification.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp).Value);
                var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

                //if (DateTime.UtcNow > expiryDate)
                //    throw new BadRequestException($"Token is expired");

                //validation: token already exist in database
                var storedToken = await _context.RefreshTokens!.FirstOrDefaultAsync(t => t.Token == request.RefreshToken);
                if (storedToken is null)
                    throw new Exception($"Token not found");

                //validation: if token is already used
                if (storedToken.IsUsed)
                    throw new Exception($"Token already used");

                //validation: if token is revoked
                if (storedToken.IsRevoked)
                    throw new Exception($"Token is revoked");

                //validation: token id
                var jti = tokenVerification.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
                if (storedToken.JwtId != jti)
                    throw new Exception($"Invalid token");

                //validation: token expiry date
                if (storedToken.ExpireDate < DateTime.UtcNow)
                    throw new Exception($"RefreshToken is expired");

                storedToken.IsUsed = true;
                _context.RefreshTokens!.Update(storedToken);
                await _context.SaveChangesAsync();

                var user = await _userManager.FindByIdAsync(storedToken.UserId);
                var token = await GenerateToken(user);          

                return new AuthResponse
                {
                    UserId = user.Id,
                    Token = token.Item1,
                    RefreshToken = token.Item2,
                    UserName = user.UserName,
                    FirstName = user.Name,
                    LastName = user.LastName,          
                };
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Lifetime validation failed. The token is expired"))
                    throw new Exception($"Token expired, try login again");
                else
                    throw new Exception(string.IsNullOrEmpty(ex.Message) ? $"Token have errors, try login again" : ex.Message);
            }
        }

        public (string, Guid)? ValidateToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "Id").Value);
                var user = jwtToken.Claims.First(x => x.Type == "UserName").Value.ToString();

                // return user id from JWT token if validation successful
                return (user, userId);
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }       

        public async Task<List<object>> GetAllUser(GetUsersParams @params)
        {
            var users = await _context.Users.ToListAsync();
            if (!users.Any())
                throw new Exception($"Users not found");

            if (@params.IsActive.HasValue)
            {
                var usersFiltered = users.Where(u => u.IsActive == @params.IsActive);
                if (!usersFiltered.Any())
                    throw new Exception($"Users not found");
                users = usersFiltered.ToList();
            }

            foreach (var user in users.ToList())
            {
                var roleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
                if (role == null)
                    continue;

                user.Role = role.NormalizedName;
                user.RoleId = role.Id;
            }

            return users.ToList<object>();
        }

        public async Task<Object> GetUserById(Guid userId)
        {
            var user = await _context.Users.Where(x => x.Id == userId.ToString()).FirstOrDefaultAsync();
            if (user == null)
                throw new Exception($"User not found");

            var roleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            if (roleName == null)
                throw new Exception("the user has no roles");
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            user.RoleId = role.Id;
            user.Role = roleName;

            return user;
        }

        public async Task<List<object>> GetUsersByIds(List<Guid> userIds)
        {
            List<object> list = new List<object>();

            if (userIds != null)
            {
                foreach (var user in userIds)
                {
                    var UsersForGuid = await _context.Users.Where(x => x.Id == user.ToString()).FirstOrDefaultAsync();
                    list.Add(UsersForGuid);
                }
            }
            else
                throw new Exception($"User not found");

            return list;
        }

        public async Task<GeneralResponse> Register(RegistrationRequest request)
        {
            var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);
            if (existingUser != null)
                return new GeneralResponse(false,$"El username proporcionado ya se encuentra en uso.");

            var existingEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existingEmail != null)
                return new GeneralResponse(false,$"El email proporcionado ya se encuentra registrado");

            var user = new ApplicationUser
            {
                Email = request.Email,
                Name = request.Name,
                LastName = request.LastName,
                UserName = request.UserName,
                EmailConfirmed = true,                
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                return new GeneralResponse(true, "Usuario registrado exitosamente");
            }
            throw new Exception($"{String.Join(" ", result.Errors.Select(s => s.Description).ToArray())}");
        }

        public async Task<string> UpdateUser(Guid userId, UpdateUserRequest updatedUser)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new Exception($"User with id {userId} was not found");

            user.Name = updatedUser.Name;
            user.UserName = updatedUser.UserName;
            user.LastName = updatedUser.LastName;
            user.Email = updatedUser.Email;
            user.IsActive = updatedUser.IsActive;

            var roleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            if (roleName == null)
                throw new Exception("the user has no role");

         

            await _userManager.UpdateAsync(user);
            return user.Id;
        }
        #endregion       

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dateTimeVal;
        }

        public async Task<string> GetUserName()
        {
            HttpContext context = _httpContextAccessor.HttpContext;
            string token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false
            };

            var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);

            var dataToken = claimsPrincipal.Claims;
            string subject = string.Empty;
            foreach (var claim in dataToken)
            {
                switch (claim.Type)
                {
                    case "UserName":
                        subject = claim.Value;
                        break;
                }
            }
            return subject;
        }
    }
}
