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
   public class CategoryService:ICategoryService
    {
        public ApplicationDbContext DbContext { get; set; }

        public CategoryService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<GlobalResponse> Add(Guid businessId, CategoryRequest request)
        {
            var business = await DbContext.Businesses.Include(x=>x.Categories).FirstOrDefaultAsync(x => x.Id == businessId);
            if (business is null)
            {
                throw new KeyNotFoundException("Business not found");
            }

            var check = business.Categories.FirstOrDefault(x => string.Equals(x.Name, request.Name, StringComparison.CurrentCultureIgnoreCase));
            if (!(check is null))
            {
                throw new AppException("Category already exist");
            }
            var category = new Category(){Name = request.Name};
            business.Categories.Add(category);
            DbContext.Update(business);
            await DbContext.SaveChangesAsync();
            return new GlobalResponse() { Message = "Successful", Status = true };
        }

        public async Task<List<Category>> GetCatergories(Guid businessId)
        {
            var business = await DbContext.Businesses.Include(x => x.Brands).FirstOrDefaultAsync(x => x.Id == businessId);
            if (business is null)
            {
                throw new KeyNotFoundException("Business not found");
            }

            var categories = await DbContext.Categories.Where(x => x.BusinessId == business.Id).ToListAsync();
            return categories;
        }
    }
}
