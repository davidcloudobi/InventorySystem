using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Response
{
   public class GlobalResponse
    {
        public string Message { get; set; } 
        public bool Status { get; set; }
    }

   public class GlobalUserResponse
   {
       public string Token { get; set; }
       public bool Status { get; set; }
       public DateTime ExpirationDate { get; set; }

   }
}
