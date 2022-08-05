using System.ComponentModel.DataAnnotations;

namespace AspNetCoreMvcPractice.ViewModels.Products
{
    public class EditProductViewModel : CreateProductViewModel
    {
        [Key]
        public int ProductID { get; set; }
    }
}
