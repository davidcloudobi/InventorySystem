using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Model;
using Domain.DTO.Request;
using Domain.Helper;
using Domain.Interface;
using Microsoft.EntityFrameworkCore;

namespace Domain.Service
{
   public class InventoryProductService: IInventoryProductService
    {
        public InventoryProductService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public ApplicationDbContext DbContext { get; set; }
        public async Task<bool> Add(InventoryProductDTO request)
        {
            var product = await DbContext.Products.SingleOrDefaultAsync(x => x.Id == request.ProductId);
            var inventory = await DbContext.Inventories.SingleOrDefaultAsync(x => x.Id == request.InventoryId);

            if (product is null)
            {
                throw new AppException("Product not found");
            }

            if (inventory is null)
            {
                throw new AppException("Product not found");
            }

            var inventoryProduct = new InventoryProduct()
                {Quantity = request.Quantity, Inventory = inventory, Product = product};
            await DbContext.InventoryProducts.AddAsync(inventoryProduct);
           await DbContext.SaveChangesAsync();

           return true;
        }
    }
}
