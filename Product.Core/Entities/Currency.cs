using i18n.Resource;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product.Core.Entities
{
    public class Currency
    {
        [Key]
        [Required(ErrorMessageResourceName = "IsRequired", ErrorMessageResourceType = typeof(Message))]
        [MaxLength(3, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Message))]
        public string CurrencyCode { get; set; }

        [Required(ErrorMessageResourceName = "IsRequired", ErrorMessageResourceType = typeof(Message))]
        [MaxLength(50, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Message))]
        [Display(Name = "CurrencyName_en", ResourceType = typeof(Label))]
        public string CurrencyName_en { get; set; } // 英文幣別名稱

        [Required(ErrorMessageResourceName = "IsRequired", ErrorMessageResourceType = typeof(Message))]
        [MaxLength(50, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Message))]
        [Display(Name = "CurrencyName_zh", ResourceType = typeof(Label))]
        public string CurrencyName_zh { get; set; } // 繁體中文幣別名稱

        [Required(ErrorMessageResourceName = "IsRequired", ErrorMessageResourceType = typeof(Message))]
        [Range(0.01, 1000, ErrorMessageResourceName = "Invalid", ErrorMessageResourceType = typeof(Message))]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal ExchangeRate { get; set; }

        [Required]
        public DateTime CreateDatetime { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedDatetime { get; set; }
    }
}
