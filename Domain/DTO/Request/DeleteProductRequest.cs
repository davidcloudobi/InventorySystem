using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Request
{
  public  class DeleteProductRequest
    {
        public IList<DeleteProductIdRequest> ProductId { get; set; }
    }

    public class DeleteProductIdRequest
    {
        public Guid ProductId { get; set; }
    }
}
