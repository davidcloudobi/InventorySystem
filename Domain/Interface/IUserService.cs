using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Model.Identity;
using Domain.DTO.Request;
using Domain.DTO.Response;

namespace Domain.Interface
{
    public interface IUserService
    {
        Task<GlobalUserResponse> Business_Add (ApplicationUser request, string password, List<string> userRole);
        Task<GlobalUserResponse> CreateUser(Guid businessId, UserRequest request);
    }
}