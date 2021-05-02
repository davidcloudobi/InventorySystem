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
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace Domain.Service
{
   public class BusinessService : IBusinessService
    {

        public BusinessService(ApplicationDbContext dbContext, IMapper mapper, UserManager<ApplicationUser> userManager, IUserService userService, IOutletService outletService)
        {
            DbContext = dbContext;
            _mapper = mapper;
            UserManager = userManager;
            UserService = userService;
            OutletService = outletService;
        }

        public ApplicationDbContext DbContext { get; set; }
        public UserManager<ApplicationUser> UserManager { get; set; }
        private readonly IMapper _mapper;
        public IUserService UserService { get; set; }
        public IOutletService OutletService { get; set; }
        public async Task<GlobalUserResponse> Add(BusinessRequest request)
        {
            var business = _mapper.Map<Business>(request);
          var businessType = await DbContext.BusinessTypes.FirstOrDefaultAsync(x => x.Name == request.BusinessTypeName);
          if (businessType == null)
          {
              throw new KeyNotFoundException("Failed Operation, business type is invalid");
          }

          var check = await DbContext.Businesses.FirstOrDefaultAsync(x => x.Name == request.Name);
          if (!(check is null))
          {
              throw new AppException("Business name already exist");
          }
            businessType.Businesses.Add(business);
            var user = new ApplicationUser
            {
                FullName = request.ContactName,
                Email = request.Email,
                UserName = request.Email,
                PhoneNumber = request.Phone,

            };
            business.ApplicationUsers.Add(user);
           var userResponse = await UserService.Business_Add(user, request.Password, new List<string>(){"AccountOwner", "Administrator" });
           //business.ApplicationUsers.Add(user);
           if (!userResponse.Status) throw new AppException("Internal Error");
           var outletResponse = await OutletService.Create(business.Id, new OutletRequest() {Name = "Main Outlet"});
           if (!outletResponse.Status) throw new AppException("Internal Error");
            DbContext.Update(business);
           await DbContext.SaveChangesAsync();
           return userResponse;

        }

        public async Task<IList<BusinessServiceResponse>> GetUsers(Guid businessId)
        {
            //var business = await DbContext.Businesses.Include(x => x.Outlets)
            //    .Include(x => x.ApplicationUsers).FirstOrDefaultAsync(x => x.Id == businessId);

            var business = await DbContext.Businesses.FirstOrDefaultAsync(x => x.Id == businessId);


            if (business == null)
            {
                throw new KeyNotFoundException("Business Id is invalid");
            }

            var applicationUsers = await UserManager.Users.Include(x=>x.Outlets).Where(x => x.BusinessId == business.Id).ToListAsync();
         
            var response = new List<BusinessServiceResponse>();
            foreach (ApplicationUser applicationUser in applicationUsers)
            {
                var roles = await UserManager.GetRolesAsync(applicationUser);
                response.Add(new BusinessServiceResponse()
                {
                    Email = applicationUser.Email,
                    UserId = applicationUser.Id,
                    Name = applicationUser.FullName,
                    Outlets = applicationUser.Outlets.Select(x=>x.Name).ToList(),
                    Roles = roles

                });
            }

            //var users = from businessUser in businessUsers
            //            let roles = (DbContext.Outlets.Where(x => Equals(x.ApplicationUsers, businessUsers))).ToList()
            //            join outlet in DbContext.Outlets on business.Id equals outlet.BusinessId


            return response;



            

        }

        public async Task<Business> GetByName(string businessName)
        {
            var business = await DbContext.Businesses.SingleOrDefaultAsync(x => x.Name == businessName);
            return business ?? throw new KeyNotFoundException("Invalid Business");
        }
    }
}
