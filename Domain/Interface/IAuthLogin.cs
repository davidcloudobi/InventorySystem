using System;
using System.Threading.Tasks;
using Domain.DTO.Request;
using Domain.DTO.Response;

namespace Domain.Interface
{
    public interface IAuthLogin
    {
        Task<AuthLoginResponse> Login(Guid businessId, AuthLoginRequest request);
    }
}