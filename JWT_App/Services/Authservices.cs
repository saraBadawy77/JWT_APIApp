using JWT_App.Helper;
using JWT_App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWT_App.Services
{
    public class Authservices : IAuthservices
    {
        private readonly UserManager<ApplaicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        public Authservices(UserManager<ApplaicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
        }

        public async Task<string> AddRoleAsync(AddRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user is null || !await _roleManager.RoleExistsAsync(model.Role))
                return "Invalid User  Id or Role Is Invalid! ";

            if (await _userManager.IsInRoleAsync(user, model.Role))
                return " The User In Role Exists ";

            var result = await _userManager.AddToRoleAsync( user,model.Role);

            return result.Succeeded ? string.Empty : "Something Is Worng ";
        }

        public async Task<AuthantationModel> GetTokenAsync(TokenRequestModel model)
        {


            var authModel = new AuthantationModel();

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.password))
            {
                authModel.Message = "Email or Password is incorrect!";
                return authModel;
            }

            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Roles = rolesList.ToList();

            return authModel;


        }

        public async Task<AuthantationModel> RegisterAsync(RegistrationModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthantationModel { Message = "This Email is already register!!" };
            if (await _userManager.FindByNameAsync(model.Username) is not null)
                return new AuthantationModel { Message = "Username is already register!!" };

            var user = new ApplaicationUser
            {
                FristName = model.FristName,
                LastName = model.LastName,
                UserName = model.Username,
                Email = model.Email

            };

            var result = await _userManager.CreateAsync(user, model.password);
            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var item in result.Errors)
                {
                    errors += $"{item.Description},";
                }

                return new AuthantationModel { Message = errors };
            }
            await _userManager.AddToRoleAsync(user, "User");

            var jwtSecurityToken = await CreateJwtToken(user);

            return new AuthantationModel
            {
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName
            };

        }


        private async Task<JwtSecurityToken> CreateJwtToken(ApplaicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

    }
}
