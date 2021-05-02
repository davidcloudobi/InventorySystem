using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Model;
using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Helper;
using Domain.Interface;
using Microsoft.EntityFrameworkCore;

namespace Domain.Service
{
   public class BrandService : IBrandService
    {
        public BrandService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public ApplicationDbContext DbContext { get; set; }
        public async Task<GlobalResponse> Add(Guid businessId, BrandRequest request)
        {
            var business = await DbContext.Businesses.Include(x=>x.Brands).FirstOrDefaultAsync(x => x.Id == businessId);
            if (business is null)
            {
                throw new KeyNotFoundException("Business not found");
            }
            var check = business.Brands.FirstOrDefault(x => string.Equals(x.Name, request.Name, StringComparison.CurrentCultureIgnoreCase));
            if (!(check is null))
            {
                throw new AppException("Brand already exist");
            }
            var brand = new Brand(){Name =  request.Name, Description =  request.Description};
            business.Brands.Add(brand);
            DbContext.Update(business);
            await DbContext.SaveChangesAsync();
            return new GlobalResponse(){Message = "Successful", Status = true};
        }

        public async Task<List<Brand>> GetBrands(Guid businessId)
        {
            var business = await DbContext.Businesses.Include(x => x.Brands).FirstOrDefaultAsync(x => x.Id == businessId);
            if (business is null)
            {
                throw new KeyNotFoundException("Business not found");
            }

            var brands = await DbContext.Brands.Where(x => x.BusinessId == business.Id).ToListAsync();
            return brands;
        }
    }
}
