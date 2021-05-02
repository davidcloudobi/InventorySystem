using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Model;
using Domain.DTO.Request;
using Domain.DTO.Response;

namespace Domain.Interface
{
    public interface ICategoryService
    {
        Task<GlobalResponse> Add(Guid businessId, CategoryRequest request);
        Task<List<Category>> GetCatergories(Guid businessId);
    }
}