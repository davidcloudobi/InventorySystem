using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Response
{
  public  class BusinessServiceResponse
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
        public IList<string> Outlets { get; set; }  
    }
}
