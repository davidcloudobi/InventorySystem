using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Request
{
  public  class InventoryProductDTO
    {
        [Required]
        public int Quantity { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public Guid InventoryId { get; set; }   
    }
}
