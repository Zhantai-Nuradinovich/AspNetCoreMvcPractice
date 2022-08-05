using AspNetCoreMvcPractice.Data.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreMvcPractice.ViewModels.Products
{
    public class CreateProductViewModel
    {
        [Required]
        [StringLength(40)]
        public string ProductName { get; set; }

        [StringLength(20)]
        public string QuantityPerUnit { get; set; }

        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be positive")]
        public decimal UnitPrice { get; set; }

        [Range(0, short.MaxValue, ErrorMessage = "The value must be positive")]
        public short UnitsInStock { get; set; }

        [Range(0, short.MaxValue, ErrorMessage = "The value must be positive")]
        public short UnitsOnOrder { get; set; }

        [Range(0, short.MaxValue, ErrorMessage = "The value must be positive")]
        public short ReorderLevel { get; set; }

        [Required]
        public bool Discontinued { get; set; }

        public int SupplierID { get; set; }
        public Supplier Supplier { get; set; }

        public int CategoryID { get; set; }
        public Category Category { get; set; }
    }
}
