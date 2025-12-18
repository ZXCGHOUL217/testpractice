using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test_practice.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название обязательно")]
        [Display(Name = "Название")]
        [StringLength(100, ErrorMessage = "Максимум 100 символов")]

        public string Name { get; set; } = string.Empty;

        [Display(Name = "Описание")]
        [StringLength(500, ErrorMessage = "Максимум 500 символов")]

        public string? Description { get; set; }

        [Required(ErrorMessage = "Цена обязательно")]
        [Display(Name = "Цена")]
        [Range(0.01, 100000, ErrorMessage = "Цена должна быть")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]

        public decimal Price { get; set; }

        [Display(Name = "Изображение")]

        public string? ImageUrl { get; set; } = "images/";

        [Display(Name = "Количество на складе")]
        [Range(0, 100, ErrorMessage = "Количество должно быть")]

        public int StockQuantity { get; set; }

        [Display(Name = "Категория")]

        public int CategoryId { get; set; }

        [Display(Name = "Категория")]

        public Category? Category { get; set; }

        [Display(Name = "Дата добавления")]

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
