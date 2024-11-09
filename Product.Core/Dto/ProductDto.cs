using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Infrastructure.Data
{

    public class BaseProduct
    {
        [Required]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Description { get; set; }
        [Range(1,9999,ErrorMessage = "價格區間只能在{1}到{2}")]
        [RegularExpression(@"[0-9]*\.?[0-9]+",ErrorMessage ="{0} 必須是數字")]
        public int Price { get; set; }
    }

    public class ProductDto : BaseProduct
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string ProductPicture { get; set; }

        public int CategoryId { get; set; }
    }

    public class ReturnProductDto
    {
        public int TotalItems { get; set; }
        public List<ProductDto> ProductDtos { get; set; }
    }

    public class CreateProductDto : BaseProduct
    { 
        public int Categoryid { get; set; }
        public IFormFile Image { get; set; }
    }

    public class UpdateProductDto : BaseProduct
    {
        public int Categoryid { get; set; }
        public string Oldimage { get; set; }
        public IFormFile Image { get; set; }
    }
}
