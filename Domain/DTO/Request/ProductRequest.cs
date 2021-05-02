using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Model;
using Microsoft.AspNetCore.Http;

namespace Domain.DTO.Request
{
public    class ProductRequest
    {
       

        [Required(ErrorMessage = "Product Name must not be empty")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Product Price must not be empty")]
        public decimal Price { get; set; }
        public DateTime? ManufacturedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        [Required(ErrorMessage = "Category Id must not be empty")]
        public Guid CategoryID { get; set; }
       
        [Required(ErrorMessage = "Brand Id must not be empty")]
        public Guid BrandID { get; set; }
        public ICollection<ProductPictureDto> ProductPictures { get; set; }
        public ICollection<ProductOutletDto> ProductOutlets { get; set; }
    }

       public class ProductPictureDto
        {
        //[Url(ErrorMessage = "Invalid Url")]
        //[Required(ErrorMessage = "Url must not be empty")]
        public IFormFile Image { get; set; }
    }


       public class ProductOutletDto
       {
           public string Name { get; set; }
           public int InventoryQuantity { get; set; }   
       }
}
