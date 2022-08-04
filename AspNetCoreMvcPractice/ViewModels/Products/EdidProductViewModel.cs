using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMvcPractice.ViewModels.Products
{
    public class EditProductViewModel : CreateProductViewModel
    {
        [Key]
        public int ProductID { get; set; }
    }
}
