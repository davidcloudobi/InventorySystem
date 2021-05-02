using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
  public class UserService:IUserService
    {
        public UserService(IOptions<AppSettings> appSettings, ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IMapper mapper, RoleManager<ApplicationRole> roleManager)
        {
            DbContext = dbContext;
            UserManager = userManager;
            _mapper = mapper;
            RoleManager = roleManager;
            _appSettings = appSettings.Value;
        }

        private readonly AppSettings _appSettings;
        public ApplicationDbContext DbContext { get; set; }
        public UserManager<ApplicationUser> UserManager { get; set; }
        public RoleManager<ApplicationRole> RoleManager { get; set; }
        private readonly IMapper _mapper;
        public async Task<GlobalUserResponse> Business_Add(ApplicationUser request, string password, List<string> userRole)
        {
           var response = await Create(request, password, userRole);
           // return new GlobalResponse(){Message = "Successful", Status = true};

           return new GlobalUserResponse()
               {Token = response.Token, ExpirationDate = response.Expiration, Status = true};

        }

        public async Task<GlobalUserResponse> CreateUser(Guid businessId, UserRequest request)
        {
            var user = _mapper.Map<ApplicationUser>(request);
            var business = await DbContext.Businesses.Include(x=>x.Outlets).FirstOrDefaultAsync(x => x.Id == businessId);
            if (business == null)
            {
                throw new KeyNotFoundException("Business not found");
            }

        
            business.ApplicationUsers.Add(user);

         var response =   await Create(user, request.Password, new List<string>() {request.Role});
            //business.ApplicationUsers.Add(user);
            foreach (var outletId in request.OutletId)
            {
                var outlet = business.Outlets.FirstOrDefault(x => x.Id == outletId);
                if (outlet == null)
                {
                    throw new KeyNotFoundException($" Outlet with the Id {outletId} not found");
                }
                user.Outlets.Add(outlet);
                DbContext.Update(user);
            }
            DbContext.Update(business);
            
             await DbContext.SaveChangesAsync();

             return new GlobalUserResponse()
                 { Token = response.Token, ExpirationDate = response.Expiration, Status = true };
            //  return new GlobalResponse() { Message = "Successful", Status = true };
        }

        private async  Task<JwtWriteResponse> Create(ApplicationUser request, string password, List<string> userRole)
        {

            var res = await UserManager.FindByEmailAsync(request.Email);
            if (res != null)
            {

                throw new AppException("Failed Operation, Email Already Exist");
            }


            foreach (var value in userRole)
            {
                var role = await RoleManager.FindByNameAsync(value);
                if (role == null)
                {
                    throw new KeyNotFoundException("Role not found");

                }
            }

            IdentityResult createdResult = await UserManager.CreateAsync(request, password);
            if (!createdResult.Succeeded)
            {
                throw new AppException(createdResult.Errors.First().Description ?? "Failed Operation, User information not valid");

            }

            foreach (var value in userRole)
            {
               
                await UserManager.AddToRoleAsync(request, value);
            }

            //###########################################################
            var response = AuthHelper.WriteJwt(request, userRole, _appSettings);
            return response;
            //#########################################################

            // await DbContext.SaveChangesAsync();
        }

    }
}
