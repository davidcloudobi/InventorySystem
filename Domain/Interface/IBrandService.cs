using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Model;
using Domain.DTO.Request;
using Domain.DTO.Response;

namespace Domain.Interface
{
    public interface IBrandService
    {
        Task<GlobalResponse> Add(Guid businessId, BrandRequest request);
        Task<List<Brand>> GetBrands(Guid businessId);
    }
}