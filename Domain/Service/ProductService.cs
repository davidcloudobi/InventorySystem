using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Data.Model;
using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Helper;
using Domain.Interface;
using Microsoft.EntityFrameworkCore;

namespace Domain.Service
{
    public  class ProductService:IProductService
    {
        public ProductService(ApplicationDbContext dbContext, IMapper mapper)
        {
            DbContext = dbContext;
            _mapper = mapper;
        }

        public ApplicationDbContext DbContext { get; set; }
        private readonly IMapper _mapper;
        public async Task<GlobalResponse> Add(Guid businessId, ProductRequest request)
        {
            var business = await DbContext.Businesses.Include(x => x. Brands).Include(x => x.Categories).Include(x => x.Outlets).Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == businessId);
            if (business is null)
            {
                throw new KeyNotFoundException("Business not found");
            }

            var category = business.Categories.FirstOrDefault(x => x.Id == request.CategoryID);
            if (category is null)
            {
                throw new KeyNotFoundException("Category not found");
            }
            var brand = business.Brands.FirstOrDefault(x => x.Id == request.BrandID);
            if (brand is null)
            {
                throw new KeyNotFoundException("Brand not found");
            }

            var check = business.Products.FirstOrDefault(x => string.Equals(x.Name, request.Name, StringComparison.CurrentCultureIgnoreCase) && x.Category.Id == request.CategoryID && x.BrandId == request.BrandID );
            if (!(check is null))
            {
                throw new AppException("Product already exist");
            }


            var product = new Product()
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                ManufacturedDate = request.ManufacturedDate,
                ExpiryDate = request.ExpiryDate,
                ProductPictures = new List<ProductPicture>()
            };
            foreach (var item in request.ProductPictures)
            {
              var result =   await new FileImageServices().Add(item.Image, product.Name);

                product.ProductPictures.Add(new ProductPicture { Url = result.FilePath });

            }

            if (product is null)
            {
                throw new AppException("Internal Error from AutoMapper");
            }
            category.Products.Add(product);
            brand.Products.Add(product);
            business.Products.Add(product);

            foreach (var productOutlet in request.ProductOutlets)
            {
                var inventoryProductItem = new InventoryProduct()
                {
                    Quantity = productOutlet.InventoryQuantity,
                    Product = product
                };
                var outlets = business.Outlets.FirstOrDefault(x => x.Name == productOutlet.Name);
                if (outlets is null) continue;
                var inventory = await DbContext.Inventories.Include(x=>x.InventoryProducts).FirstOrDefaultAsync(x => x.OutletId == outlets.Id);
                if (inventory is null) throw new KeyNotFoundException($"No inventory found for {outlets.Name} ");
                inventory.InventoryProducts.Add(inventoryProductItem);
               inventory.Quantity += inventoryProductItem.Quantity;
               product.Quantity += inventoryProductItem.Quantity;
            }

            DbContext.Update(product);
           // product.Quantity = product.InventoryProducts.Sum(x => x.Quantity);
            DbContext.Update(business);
            await DbContext.SaveChangesAsync();
            return new GlobalResponse() {Message = "Successful", Status = true};

        }

        public async Task<IList<Product>> GetProducts(Guid businessId)
        {
            var business = await DbContext.Businesses.Include(x=>x.Products).SingleOrDefaultAsync(x => x.Id == businessId);
            if (business is null)
            {
                throw new KeyNotFoundException("Invalid Business Id");
            }

            return business.Products.ToList();

        }

        public async Task<IList<Product>> FilterProducts(string name)
        {
            var products = await DbContext.Products.Include(x=>x.InventoryProducts).Where(x => x.Name.Contains(name)).ToListAsync();
            return products;
        }

        public async Task<Product> GetProduct(Guid businessId, Guid productId)
        {
            var business = await DbContext.Businesses.Include(x => x.Products).SingleOrDefaultAsync(x => x.Id == businessId);
            if (business is null)
            {
                throw new KeyNotFoundException("Invalid Business Id");
            }

            var product = await DbContext.Products.SingleOrDefaultAsync(x => x.Id == productId && x.BusinessId == business.Id);
            return product;
        }

        public async Task<GlobalResponse> UpdatePrice(Guid businessId, Guid productId, UpdateProductPrice price)
        {
            var business = await DbContext.Businesses.Include(x => x.Products).SingleOrDefaultAsync(x => x.Id == businessId);
            if (business is null)
            {
                throw new KeyNotFoundException("Invalid Business Id");
            }

            var product = await DbContext.Products.SingleOrDefaultAsync(x => x.Id == productId && x.BusinessId == business.Id);
            product.Price = price.NewPrice;
            DbContext.Update(product);
            await DbContext.SaveChangesAsync();

            return new GlobalResponse{ Message = "successful", Status= true};

        }

        public async Task<GlobalResponse> DeleteOneProduct(Guid businessId, Guid productId)
        {

            var business = await DbContext.Businesses.Include(x => x.Products).SingleOrDefaultAsync(x => x.Id == businessId);
            if (business is null)
            {
                throw new KeyNotFoundException("Invalid Business Id");
            }

            var product = await DbContext.Products.SingleOrDefaultAsync(x => x.Id == productId && x.BusinessId == business.Id);
            DbContext.Remove(product);
            await DbContext.SaveChangesAsync();
            return new GlobalResponse { Message = "successful", Status = true };
        }

        public async Task<List<GlobalResponse>> DeleteProducts(Guid businessId, DeleteProductRequest request)
        {
            var business = await DbContext.Businesses.Include(x => x.Products).SingleOrDefaultAsync(x => x.Id == businessId);
            if (business is null)
            {
                throw new KeyNotFoundException("Invalid Business Id");
            }
            var returnValue = new List<GlobalResponse>();

            foreach (var product in request.ProductId)
            {
                var responseProduct = await DbContext.Products.SingleOrDefaultAsync(x => x.Id == product.ProductId && x.BusinessId == business.Id);
                var id = responseProduct.Id;
                DbContext.Remove(responseProduct);
              var deleted =   await DbContext.SaveChangesAsync();
                if (deleted > 0)
                {
                    returnValue.Add(new GlobalResponse { Message = $"product with id {id} successful", Status = true });
                }
            }
            return returnValue;
        }
    }
}
