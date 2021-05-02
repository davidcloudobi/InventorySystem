using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Model;
using Data.Model.Identity;
using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Helper;
using Domain.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Domain.Service
{
    public class AuthLoginService: IAuthLogin
    {
        public AuthLoginService(IOptions<AppSettings> appSettings, ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            DbContext = dbContext;
            UserManager = userManager;
            RoleManager = roleManager;
            SignInManager = signInManager;
            _appSettings = appSettings.Value;
        }

        private readonly AppSettings _appSettings;
        public ApplicationDbContext DbContext { get; set; }
        public UserManager<ApplicationUser> UserManager { get; set; }
        public RoleManager<ApplicationRole> RoleManager { get; set; }
        public SignInManager<ApplicationUser> SignInManager { get; set; }   
        public async Task<AuthLoginResponse> Login(Guid businessId, AuthLoginRequest request)
        {
            var user = await UserManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new KeyNotFoundException("Invalid Email");
            }

            var check =  await SignInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (!check.Succeeded)
            {
                throw new AppException("Invalid Password");
            }

            if (user.BusinessId != businessId)
            {
                throw new AppException("Invalid, User not registered to this business");
            }

            var roles = await UserManager.GetRolesAsync(user);


            var response = AuthHelper.WriteJwt(user, roles, _appSettings);

            var returnValue = new AuthLoginResponse()
            {
                Email = user.Email,
                Name = user.FullName,
                Id = user.Id,
                PhoneNumber = user.PhoneNumber,
                Token = response.Token,
                TokenExpirationDate = response.Expiration
            };

            return returnValue;



        }
    }
}
