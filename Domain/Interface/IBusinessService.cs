using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Model;
using Domain.DTO.Request;
using Domain.DTO.Response;

namespace Domain.Interface
{
  public  interface  IBusinessService
  {
      Task<GlobalUserResponse> Add( BusinessRequest request);

      Task<IList<BusinessServiceResponse>> GetUsers(Guid businessId);

      Task<Business> GetByName(string businessName);



  }
}
