using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Model;
using Domain.DTO.Request;
using Domain.DTO.Response;

namespace Domain.Interface
{
    public interface IProductService
    {
        Task<GlobalResponse> Add(Guid businessId, ProductRequest request);
        Task<IList<Product>> GetProducts(Guid businessId);
        Task<IList<Product>> FilterProducts(string name);

        Task<Product> GetProduct(Guid businessId, Guid productId);

        Task<GlobalResponse> UpdatePrice(Guid businessId, Guid productId, UpdateProductPrice price);

        Task<GlobalResponse> DeleteOneProduct(Guid businessId, Guid productId);

        Task<List<GlobalResponse>> DeleteProducts(Guid businessId, DeleteProductRequest request);



    }
}